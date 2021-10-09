using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuHandler"/> is responsible for managing and transitioning <see cref="MenuHandler"/>.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Menu Handler"), RequireComponent(typeof(CanvasGroup)), DefaultExecutionOrder(-50), DisallowMultipleComponent]
    public class MenuHandler : UIBehaviour, IMenuHandler
    {
        [SerializeField] private MenuScreen initialScreen = default;

        /// <summary>
        /// Reference to the <see cref="RectTransform"/> component of the handler.
        /// </summary>
        public RectTransform RectTransform { get; private set; }

        /// <summary>
        /// Reference to the <see cref="Canvas"/> component of the handler.
        /// </summary>
        public Canvas Canvas { get; private set; }

        /// <summary>
        /// The event system used by this handler.
        /// </summary>
        public EventSystem EventSystem { get; private set; }

        /// <summary>
        /// The canvas group of the handler.
        /// </summary>
        public CanvasGroup CanvasGroup { get; private set; }

        /// <summary>
        /// Used to control visibility of all screens managed by this handler.
        /// </summary>
        public bool Visible
        {
            get
            {
                return CanvasGroup.alpha > 0.0f;
            }
            set
            {
                CanvasGroup.alpha = value ? 1.0f : 0.0f;
            }
        }

        /// <summary>
        /// An array containing all of the screens available to this handler.
        /// </summary>
        private IMenuScreen[] Screens { get; set; }

        /// <summary>
        /// The current screen stack.
        /// </summary>
        public Stack<IMenuScreen> ScreenStack { get; private set; }

        /// <summary>
        /// The current screen if there is one on the stack.
        /// </summary>
        public IMenuScreen CurrentScreen
        {
            get
            {
                if (!HasScreen)
                {
                    return null;
                }
                return ScreenStack.Peek();
            }
        }

        /// <summary>
        /// True if there is any screens on the stack.
        /// </summary>
        public bool HasScreen
        {
            get
            {
                return ScreenStack != null && ScreenStack.Count > 0;
            }
        }

        /// <summary>
        /// Controls the cursor lock and visibility states.
        /// </summary>
        public bool CursorLocked
        {
            get
            {
                return Cursor.lockState == CursorLockMode.Locked && !Cursor.visible;
            }
            set
            {
                Cursor.visible = !value;
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }

        /// <summary>
        /// Returns the <see cref="IMenuAlert"/> if it exists.
        /// </summary>
        public IMenuAlert Alert { get; private set; }

        /// <summary>
        /// Returns the <see cref="IMenuDialogue"/> if it exists.
        /// </summary>
        public IMenuDialogue Dialogue { get; private set; }

        /// <summary>
        /// Returns the <see cref="IMenuTooltip"/> if it exists.
        /// </summary>
        public IMenuTooltip Tooltip { get; private set; }

        /// <summary>
        /// Called when a transition between two <see cref="IMenuScreen"/>s begins (Ordered: Out Screen, In Screen).
        /// </summary>
        public event Action<IMenuScreen, IMenuScreen> OnScreenStateWillChangeEvent;

        /// <summary>
        /// Called when a transition between two <see cref="IMenuScreen"/>s is completed (Ordered: Out Screen, In Screen).
        /// </summary>
        public event Action<IMenuScreen, IMenuScreen> OnScreenStateDidChangeEvent;

        protected override void Awake()
        {
            base.Awake();
            ScreenStack = new Stack<IMenuScreen>();
            RectTransform = GetComponent<RectTransform>();
            Canvas = GetComponent<Canvas>();
            EventSystem = EventSystem.current;
            CanvasGroup = GetComponent<CanvasGroup>();
            Screens = GetComponentsInChildren<IMenuScreen>(true);
            for (int i = 0; i < Screens.Length; i++)
            {
                Screens[i].Initialize(this);
            }
            ScreenStack = new Stack<IMenuScreen>();
            Dialogue = GetScreen<MenuDialogue>();
            Alert = GetComponentInChildren<MenuAlert>(true);
            Tooltip = GetComponentInChildren<MenuTooltip>(true);
            if (Tooltip != null)
            {
                IMenuTooltipData[] tooltipData = GetComponentsInChildren<IMenuTooltipData>(true);
                for (int i = 0; i < tooltipData.Length; i++)
                {
                    tooltipData[i].Initialize(Tooltip);
                }
            }
            if (initialScreen != null)
            {
                PushScreen(initialScreen);
            }
        }

        public List<RaycastResult> Raycast(Vector2 position)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.RaycastAll(pointerEventData, results);
            return results;
        }

        private void Update()
        {
            if (HasScreen)
            {
                CurrentScreen.OnActive();
            }
            if (Alert != null)
            {
                Alert.UpdateTimer(Time.unscaledDeltaTime);
            }
            if (Tooltip != null)
            {
#if ENABLE_INPUT_SYSTEM
                Vector2 mousePosition = Mouse.current.position.ReadValue();
#else
                Vector2 mousePosition = Input.mousePosition;
#endif
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    RectTransform, 
                    mousePosition,
                    Canvas.renderMode != RenderMode.ScreenSpaceOverlay ? Canvas.worldCamera : null, 
                    out Vector2 localPoint);
                if (localPoint.x > 0.0f)
                {
                    Tooltip.PivotRight();
                } else
                {
                    Tooltip.PivotLeft();
                }
                Tooltip.SetPosition(localPoint);
            }
        }

        /// <summary>
        /// Pushes a <see cref="IMenuScreen"/> to the stack, transitioning it in and the old one out.
        /// </summary>
        public IPromise PushScreen(IMenuScreen menuScreen, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? CurrentScreen : null;
            ScreenStack.Push(menuScreen);
            return TransitionScreens(outScreen, menuScreen, options);
        }

        /// <summary>
        /// Pops the <see cref="IMenuScreen"/> at the top of the stack off, transitioning it out and the next available one in.
        /// </summary>
        public IPromise PopScreen(in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            IMenuScreen inScreen = HasScreen ? CurrentScreen : null;
            return TransitionScreens(outScreen, inScreen, options);
        }

        /// <summary>
        /// Pops the <see cref="IMenuScreen"/> at the top of the stack off, transitioning it out and the requested one in.
        /// </summary>
        public IPromise PopPushScreen(IMenuScreen menuScreen, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Push(menuScreen);
            return TransitionScreens(outScreen, menuScreen, options);
        }

        /// <summary>
        /// Pops to the requested <see cref="IMenuScreen"/> in the stack.
        /// </summary>
        private void PopToScreen(IMenuScreen menuScreen, IPromise promise, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            if (CurrentScreen == menuScreen)
            {
                promise.Resolve();
                return;
            }
            IMenuScreen outScreen = CurrentScreen;
            while (ScreenStack.Count > 0 && ScreenStack.Peek() != menuScreen)
            {
                ScreenStack.Pop();
            }
            IMenuScreen inScreen = ScreenStack.Peek();
            TransitionScreens(outScreen, inScreen, options).Catch(promise.Reject).Done(promise.Resolve);
        }

        /// <summary>
        /// Pops to the requested <see cref="IMenuScreen"/> if it's present in the stack, returns a <see cref="IPromise"/> 
        /// that will resolve on completion, or reject if the <see cref="IMenuScreen"/> is not in the stack.
        /// </summary>
        public IPromise PopToScreen(IMenuScreen menuScreen, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IPromise promise = Promise.Create();
            if (!ScreenStack.Contains(menuScreen))
            {
                promise.Reject(new InvalidOperationException(string.Format("The Menu Screen '{0}' is not in the current screen stack.", menuScreen)));
                return promise;
            }
            PopToScreen(menuScreen, promise, options);
            return promise;
        }

        /// <summary>
        /// Pops all <see cref="IMenuScreen"/>s off the stack, optionally pushing a new one in their place.
        /// </summary>
        public IPromise PopAllScreens(IMenuScreen menuScreen = null, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Clear();
            if (menuScreen != null)
            {
                ScreenStack.Push(menuScreen);
            }
            return TransitionScreens(outScreen, menuScreen, options);
        }

        /// <summary>
        /// Forces the stack into a new state containing only the requested <see cref="IMenuScreen"/>.
        /// </summary>
        public IPromise SetScreenStack(IMenuScreen menuScreens, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            return SetScreenStack(new IMenuScreen[] { menuScreens }, options);
        }

        /// <summary>
        /// Forces the stack into a new state containing the requested <see cref="MenuScreen"/>s (the final <see cref="MenuScreen"/> in the array will be at the top of the stack).
        /// </summary>
        public IPromise SetScreenStack(IMenuScreen[] menuScreens, in MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Clear();
            for (int i = 0; i < menuScreens.Length; i++)
            {
                ScreenStack.Push(menuScreens[i]);
            }
            IMenuScreen inScreen = menuScreens[menuScreens.Length - 1];
            return TransitionScreens(outScreen, inScreen, options);
        }

        /// <summary>
        /// Returns a reference to the Component of Type 'T' if any of the <see cref="IMenuScreen"/>s available to this <see cref="MenuHandler"/>s are of Type 'T'.
        /// </summary>
        public T GetScreen<T>() where T : IMenuScreen
        {
            for (int i = 0; i < Screens.Length; i++)
            {
                if (typeof(T).IsAssignableFrom(Screens[i].GetType()))
                {
                    return (T)Screens[i];
                }
            }
            return default;
        }

        /// <summary>
        /// Returns a <see cref="IPromise"/> that resolves when both screens complete their transitions basing the sequence on the specified option.
        /// </summary>
        private IPromise TransitionScreens(IMenuScreen outScreen, IMenuScreen inScreen, MenuTransitionOptions options = MenuTransitionOptions.Parallel)
        {
            if (outScreen != null && inScreen != null)
            {
                if (outScreen.Equals(inScreen))
                {
                    return Promise.Resolved();
                }
            }

            IPromise promise = Promise.Create();

            OnScreenStateWillChangeEvent?.Invoke(outScreen, inScreen);
            void Resolve()
            {
                OnScreenStateDidChangeEvent?.Invoke(outScreen, inScreen);
                promise.Resolve();
            }

            if (inScreen == null)
            {
                if (outScreen == null)
                {
                    Resolve();
                    return promise;
                }
                outScreen.TransitionOut(options.HasFlag(MenuTransitionOptions.OutInstant)).Done(Resolve);
                return promise;
            }

            if (outScreen == null)
            {
                inScreen.TransitionIn(options.HasFlag(MenuTransitionOptions.InInstant)).Done(Resolve);
                return promise;
            }

            if (options.HasFlag(MenuTransitionOptions.Sequential))
            {
                outScreen.TransitionOut(options.HasFlag(MenuTransitionOptions.OutInstant)).Done(() =>
                {
                    inScreen.TransitionIn(options.HasFlag(MenuTransitionOptions.InInstant)).Done(Resolve);
                });
                return promise;
            }

            int resolvedCount = 0;
            void TryResolve()
            {
                resolvedCount++;
                if (resolvedCount == 2)
                {
                    Resolve();
                }
            }
            outScreen.TransitionOut(options.HasFlag(MenuTransitionOptions.OutInstant)).Done(TryResolve);
            inScreen.TransitionIn(options.HasFlag(MenuTransitionOptions.InInstant)).Done(TryResolve);

            return promise;
        }
    }
}
