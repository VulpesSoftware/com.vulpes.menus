using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
                return (Cursor.lockState == CursorLockMode.Locked && !Cursor.visible);
            }
            set
            {
                Cursor.visible = !value;
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }

        /// <summary>
        /// Returns the Menu Alert if it exists.
        /// </summary>
        public IMenuAlert Alert { get; private set; }

        /// <summary>
        /// Returns the Dialogue Menu Screen if it exists.
        /// </summary>
        public IMenuDialogue Dialogue { get; private set; }

        /// <summary>
        /// Returns the Menu Tooltip if it exists.
        /// </summary>
        public IMenuTooltip Tooltip { get; private set; }

        /// <summary>
        /// Called when a transition between two Menu Screens begins (Ordered: Out Screen, In Screen).
        /// </summary>
        public event Action<IMenuScreen, IMenuScreen> OnScreenStateWillChangeEvent;

        /// <summary>
        /// Called when a transition between two Menu Screens is completed (Ordered: Out Screen, In Screen).
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
            if (initialScreen != null)
            {
                PushScreen(initialScreen);
            }
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
            if (Tooltip != null && Tooltip.IsActive)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    RectTransform, 
                    Input.mousePosition,
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
        /// Pushes a Menu Screen to the stack, transitioning it in and the old one out.
        /// </summary>
        public IPromise PushScreen(IMenuScreen akMenuScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? CurrentScreen : null;
            ScreenStack.Push(akMenuScreen);
            return TransitionScreens(outScreen, akMenuScreen, akOptions);
        }

        /// <summary>
        /// Pops the Menu Screen at the top of the stack off, transitioning it out and the next available one in.
        /// </summary>
        public IPromise PopScreen(MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            IMenuScreen inScreen = HasScreen ? CurrentScreen : null;
            return TransitionScreens(outScreen, inScreen, akOptions);
        }

        /// <summary>
        /// Pops the Menu Screen at the top of the stack off, transitioning it out and the requested one in.
        /// </summary>
        public IPromise PopPushScreen(IMenuScreen akMenuScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Push(akMenuScreen);
            return TransitionScreens(outScreen, akMenuScreen, akOptions);
        }

        /// <summary>
        /// Pops to the requested Menu Screen in the stack.
        /// </summary>
        private void PopToScreen(IMenuScreen akMenuScreen, IPromise akPromise, MenuTransitionOptions akOptions)
        {
            if (CurrentScreen == akMenuScreen)
            {
                akPromise.Resolve();
                return;
            }
            IMenuScreen outScreen = CurrentScreen;
            while (ScreenStack.Count > 0 && ScreenStack.Peek() != akMenuScreen)
            {
                ScreenStack.Pop();
            }
            IMenuScreen inScreen = ScreenStack.Peek();
            TransitionScreens(outScreen, inScreen, akOptions).Catch(akPromise.Reject).Done(akPromise.Resolve);
        }

        /// <summary>
        /// Pops to the requested Menu Screen if it's present in the stack, returns a Promise 
        /// that will resolve on completion, or reject if the Menu Screen is not in the stack.
        /// </summary>
        public IPromise PopToScreen(IMenuScreen akMenuScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            IPromise promise = Promise.Create();
            if (!ScreenStack.Contains(akMenuScreen))
            {
                promise.Reject(new InvalidOperationException(string.Format("The Menu Screen '{0}' is not in the current screen stack.", akMenuScreen)));
                return promise;
            }
            PopToScreen(akMenuScreen, promise, akOptions);
            return promise;
        }

        /// <summary>
        /// Pops all Menu Screens off the stack, optionally pushing a new one in their place.
        /// </summary>
        public IPromise PopAllScreens(IMenuScreen akMenuScreen = null, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Clear();
            if (akMenuScreen != null)
            {
                ScreenStack.Push(akMenuScreen);
            }
            return TransitionScreens(outScreen, akMenuScreen, akOptions);
        }

        /// <summary>
        /// Forces the stack into a new state containing only the requested Menu Screen.
        /// </summary>
        public IPromise SetScreenStack(IMenuScreen akMenuScreens, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            return SetScreenStack(new IMenuScreen[] { akMenuScreens }, akOptions);
        }

        /// <summary>
        /// Forces the stack into a new state containing the requested Menu Screens (the final Menu Screen in the array will be at the top of the stack).
        /// </summary>
        public IPromise SetScreenStack(IMenuScreen[] akMenuScreens, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Clear();
            for (int i = 0; i < akMenuScreens.Length; i++)
            {
                ScreenStack.Push(akMenuScreens[i]);
            }
            IMenuScreen inScreen = akMenuScreens[akMenuScreens.Length - 1];
            return TransitionScreens(outScreen, inScreen, akOptions);
        }

        /// <summary>
        /// Returns a reference to the Component of Type 'T' if any of the Menu Screens available to this Menu Handler are of Type 'T'.
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
        /// Returns a Promise that resolves when both screens complete their transitions basing the sequence on the specified option.
        /// </summary>
        private IPromise TransitionScreens(IMenuScreen akOutScreen, IMenuScreen akInScreen, MenuTransitionOptions akOptions = MenuTransitionOptions.Parallel)
        {
            if (akOutScreen != null && akInScreen != null)
            {
                if (akOutScreen.Equals(akInScreen))
                {
                    return Promise.Resolved();
                }
            }

            IPromise promise = Promise.Create();

            OnScreenStateWillChangeEvent?.Invoke(akOutScreen, akInScreen);
            void Resolve()
            {
                OnScreenStateDidChangeEvent?.Invoke(akOutScreen, akInScreen);
                promise.Resolve();
            }

            if (akInScreen == null)
            {
                if (akOutScreen == null)
                {
                    Resolve();
                    return promise;
                }
                akOutScreen.TransitionOut(akOptions.HasFlag(MenuTransitionOptions.OutInstant)).Done(Resolve);
                return promise;
            }

            if (akOutScreen == null)
            {
                akInScreen.TransitionIn(akOptions.HasFlag(MenuTransitionOptions.InInstant)).Done(Resolve);
                return promise;
            }

            if (akOptions.HasFlag(MenuTransitionOptions.Sequential))
            {
                akOutScreen.TransitionOut(akOptions.HasFlag(MenuTransitionOptions.OutInstant)).Done(() =>
                {
                    akInScreen.TransitionIn(akOptions.HasFlag(MenuTransitionOptions.InInstant)).Done(Resolve);
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
            akOutScreen.TransitionOut(akOptions.HasFlag(MenuTransitionOptions.OutInstant)).Done(TryResolve);
            akInScreen.TransitionIn(akOptions.HasFlag(MenuTransitionOptions.InInstant)).Done(TryResolve);

            return promise;
        }
    }
}
