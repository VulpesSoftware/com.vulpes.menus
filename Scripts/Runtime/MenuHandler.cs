using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuHandler"/> is responsible for managing and transitioning <see cref="MenuScreen"/>s.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Menu Handler"), RequireComponent(typeof(Canvas)), DefaultExecutionOrder(-50), DisallowMultipleComponent]
    public class MenuHandler : UIBehaviour, IMenuHandler
    {
        [SerializeField] private MenuScreen initialScreen = default;

        private Canvas canvas;
        private IMenuScreen[] screens;

        /// <summary>
        /// Used to control visibility of all screens managed by this handler.
        /// </summary>
        public bool Visible
        {
            get => canvas.enabled;
            set => canvas.enabled = value;
        }

        /// <summary>
        /// True if there is any screens on the stack.
        /// </summary>
        protected bool HasScreen => ScreenStack != null && ScreenStack.Count > 0;

        public Stack<IMenuScreen> ScreenStack { get; protected set; }

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
        /// Controls the cursor lock and visibility states.
        /// </summary>
        public bool CursorLocked
        {
            get => Cursor.lockState == CursorLockMode.Locked && !Cursor.visible;
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
        /// Returns the <see cref="IMenuLoading"/> if it exists.
        /// </summary>
        public IMenuLoading Loading { get; private set; }

        /// <summary>
        /// Returns the <see cref="IMenuModal"/> if it exists.
        /// </summary>
        public IMenuModal Modal { get; private set; }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            switch (initialScreen)
            {
                case IMenuLoading:
                case IMenuModal:
                    Debug.LogWarning($"Cannot use '{initialScreen.GetType().Name}' as the initial screen because it's a special screen type.");
                    initialScreen = null;
                    break;
                default:
                    break;
            }
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            ScreenStack = new();
            canvas = GetComponent<Canvas>();
            screens = GetComponentsInChildren<IMenuScreen>(true);
            for (int i = screens.Length - 1; i >= 0; i--)
            {
                screens[i].Initialize(this);
            }
            Modal = GetComponentInChildren<IMenuModal>(true);
            Loading = GetComponentInChildren<IMenuLoading>(true);
            Alert = GetComponentInChildren<IMenuAlert>(true);
            Alert.Initialize();
            if (initialScreen != null)
            {
                PushScreen(initialScreen);
            }
        }

        private void UpdateCurrentScreen()
        {
            if (!HasScreen)
            {
                return;
            }
            CurrentScreen?.Active();
        }

        private void UpdateAlert()
        {
            if (Alert == null)
            {
                return;
            }
            Alert.UpdatePromiseTimer(Time.unscaledDeltaTime);
        }

        protected virtual void Update()
        {
            UpdateCurrentScreen();
            UpdateAlert();
        }

        /// <summary>
        /// Returns a <see cref="IPromise"/> that resolves when both screens complete their transitions basing the sequence on the specified option.
        /// </summary>
        private IPromise TransitionScreens(IMenuScreen outScreen, IMenuScreen inScreen, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
        {
            if (outScreen != null && inScreen != null)
            {
                if (outScreen.Equals(inScreen))
                {
                    return Promise.Resolved();
                }
            }

            IPromise promise = Promise.Create();
            bool inInstant = options.HasFlag(MenuTransitionOptions.InInstant);
            bool outInstant = options.HasFlag(MenuTransitionOptions.OutInstant);

            if (inScreen == null)
            {
                if (outScreen == null)
                {
                    promise.Resolve();
                    return promise;
                }
                outScreen.TransitionOut(new(outInstant, null)).Done(promise.Resolve);
                return promise;
            }

            if (outScreen == null)
            {
                inScreen.TransitionIn(new(inInstant, null)).Done(promise.Resolve);
                return promise;
            }

            if (options.HasFlag(MenuTransitionOptions.Sequential))
            {
                outScreen.TransitionOut(new(outInstant, inScreen)).Done(() =>
                {
                    inScreen.TransitionIn(new(inInstant, outScreen)).Done(promise.Resolve);
                });
                return promise;
            }

            int resolvedCount = 0;
            void TryResolve()
            {
                resolvedCount++;
                if (resolvedCount == 2)
                {
                    promise.Resolve();
                }
            }

            outScreen.TransitionOut(new(outInstant, inScreen)).Done(TryResolve);
            inScreen.TransitionIn(new(inInstant, outScreen)).Done(TryResolve);

            return promise;
        }

        /// <summary>
        /// Pushes a <see cref="IMenuScreen"/> to the stack, transitioning it in and the old one out.
        /// </summary>
        public IPromise PushScreen(IMenuScreen menuScreen, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
        {
            if (menuScreen == null)
            {
                return Promise.Rejected(new("Can't push 'null' to the screen stack."));
            }
            IMenuScreen outScreen = CurrentScreen;
            ScreenStack.Push(menuScreen);
            return TransitionScreens(outScreen, menuScreen, options);
        }

        /// <summary>
        /// Pops the <see cref="IMenuScreen"/> at the top of the stack off, transitioning it out and the next available one in.
        /// </summary>
        public IPromise PopScreen(MenuTransitionOptions options = MenuTransitionOptions.Sequential)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            IMenuScreen inScreen = CurrentScreen;
            return TransitionScreens(outScreen, inScreen, options);
        }

        /// <summary>
        /// Pops the <see cref="IMenuScreen"/> at the top of the stack off, transitioning it out and the requested one in.
        /// </summary>
        public IPromise PopPushScreen(IMenuScreen menuScreen, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
        {
            if (menuScreen == null)
            {
                return Promise.Rejected(new("Can't push 'null' to the screen stack."));
            }
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Push(menuScreen);
            return TransitionScreens(outScreen, menuScreen, options);
        }

        /// <summary>
        /// Pops to the requested <see cref="IMenuScreen"/> in the stack.
        /// </summary>
        private void PopToScreenInternal(IMenuScreen menuScreen, IPromise promise, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
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
        public IPromise PopToScreen(IMenuScreen menuScreen, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
        {
            if (!ScreenStack.Contains(menuScreen))
            {
                return Promise.Rejected(new($"The screen '{menuScreen}' is not in the screen stack."));
            }
            IPromise promise = Promise.Create();
            PopToScreenInternal(menuScreen, promise, options);
            return promise;
        }

        /// <summary>
        /// Pops all <see cref="IMenuScreen"/>s off the stack, optionally pushing a new one in their place.
        /// </summary>
        public IPromise PopAllScreens(IMenuScreen menuScreen = null, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
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
        public IPromise SetScreenStack(IMenuScreen menuScreens, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
            => SetScreenStack(new IMenuScreen[] { menuScreens }, options);

        /// <summary>
        /// Forces the stack into a new state containing the requested <see cref="MenuScreen"/>s (the final <see cref="MenuScreen"/> in the array will be at the top of the stack).
        /// </summary>
        public IPromise SetScreenStack(IMenuScreen[] menuScreens, MenuTransitionOptions options = MenuTransitionOptions.Sequential)
        {
            IMenuScreen outScreen = HasScreen ? ScreenStack.Pop() : null;
            ScreenStack.Clear();
            for (int i = 0; i < menuScreens.Length; i++)
            {
                if (menuScreens[i] == null)
                {
                    continue;
                }
                ScreenStack.Push(menuScreens[i]);
            }
            IMenuScreen inScreen = menuScreens[^1];
            return TransitionScreens(outScreen, inScreen, options);
        }

        /// <summary>
        /// Returns a reference to the Component of Type 'T' if any of the <see cref="IMenuScreen"/>s available to this <see cref="MenuHandler"/>s are of Type 'T'.
        /// </summary>
        public T GetScreen<T>() where T : IMenuScreen
        {
            for (int i = screens.Length - 1; i >= 0; i--)
            {
                if (typeof(T).IsAssignableFrom(screens[i].GetType()))
                {
                    return (T)screens[i];
                }
            }
            return default;
        }
    }
}