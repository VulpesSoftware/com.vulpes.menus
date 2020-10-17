using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// Base class for all <see cref="MenuScreen"/>, contains controls for Cursor Visibility state as well as the Transition type.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup)), DisallowMultipleComponent]
    public abstract class MenuScreen : UIBehaviour, IMenuScreen
    {
        protected CanvasGroup canvasGroup;

        [SerializeField, Tooltip("If true the mouse cursor will be unlocked on this screen.")] 
        protected bool showCursor = false;

        [SerializeField, Tooltip("If true this screen will remember the previous selection and use it as the default.")] 
        protected bool rememberSelection = false;

        [SerializeField, Tooltip("If assigned this will be the default selection when this menu appears.")] 
        protected Selectable defaultSelection = default;

        [SerializeField, Tooltip("If assigned the menu will use this transition when transitioning in and out.")] 
        protected MenuTransition transition = default;

        /// <summary>
        /// Called when the <see cref="MenuScreen"/> will appear (executed after the <see cref="OnWillAppear"/> method for this screen).
        /// </summary>
        public event Action OnWillAppearEvent;

        /// <summary>
        /// Called when the <see cref="MenuScreen"/> has finished appearing (executed after the <see cref="OnDidAppear"/> method for this screen).
        /// </summary>
        public event Action OnDidAppearEvent;

        /// <summary>
        /// Called when the <see cref="MenuScreen"/> will disappear (executed after the <see cref="OnWillDisappear"/> method for this screen).
        /// </summary>
        public event Action OnWillDisappearEvent;

        /// <summary>
        /// Called when the <see cref="MenuScreen"/> has finished disappearing (executed after the <see cref="OnDidDisappear"/> method for this screen).
        /// </summary>
        public event Action OnDidDisappearEvent;

        /// <summary>
        /// Called when the state of this <see cref="MenuScreen"/> changes.
        /// </summary>
        public event Action<MenuScreenState, MenuScreenState> OnStateChangedEvent;

        private IPromise transitionInPromise;
        private IPromise transitionOutPromise;

        public IMenuHandler MenuHandler { get; protected set; }

        public MenuScreenState State { get; protected set; }

        public bool IsTransitioning
        {
            get
            {
                return State == MenuScreenState.TransitioningIn || State == MenuScreenState.TransitioningOut;
            }
        }

        public bool Interactable
        {
            get
            {
                return canvasGroup.interactable;
            }
            set
            {
                canvasGroup.interactable = value;
            }
        }

        public bool BlocksRaycasts
        {
            get
            {
                return canvasGroup.blocksRaycasts;
            }
            set
            {
                canvasGroup.blocksRaycasts = value;
            }
        }

        public Selectable DefaultSelection { get; set; }

        public bool IsCurrentScreen
        {
            get
            {
                if (MenuHandler == null)
                {
                    Debug.LogError("No Menu Handler Assigned!");
                    return false;
                }
                return MenuHandler.CurrentScreen == (object)this;
            }
        }

        /// <summary>
        /// Initializes the <see cref="MenuScreen"/>.
        /// </summary>
        public void Initialize(IMenuHandler akMenuHandler)
        {
            MenuHandler = akMenuHandler;
            canvasGroup = GetComponent<CanvasGroup>();
            State = MenuScreenState.Out;
            Interactable = false;
            BlocksRaycasts = false;
            DefaultSelection = defaultSelection;
            gameObject.SetActive(false);
            OnInitialize();
        }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> is first initialized by the <see cref="MenuHandler"/>.
        /// </summary>
        public virtual void OnInitialize()
        {

        }

        /// <summary>
        /// This method executes every frame that this <see cref="MenuScreen"/> is the active.
        /// </summary>
        public virtual void OnActive()
        {

        }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> begins transitioning in.
        /// </summary>
        public virtual void OnWillAppear()
        {

        }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> finishes transitioning in.
        /// </summary>
        public virtual void OnDidAppear()
        {

        }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> begins transitioning out.
        /// </summary>
        public virtual void OnWillDisappear()
        {

        }

        /// <summary>
        /// Called on when this <see cref="MenuScreen"/> finishes transitioning out.
        /// </summary>
        public virtual void OnDidDisappear()
        {

        }

        /// <summary>
        /// Returns a <see cref="Promise"/> that resolves when this <see cref="MenuScreen"/> finishes transitioning in.
        /// </summary>
        public IPromise TransitionIn(bool abInstant = false)
        {
            if (transitionInPromise != null)
            {
                return transitionInPromise;
            }
            transitionInPromise = Promise.Create();
            IPromise result = transitionInPromise;
            gameObject.SetActive(true);
            MenuHandler.CursorLocked = !showCursor;
            // TBH: This is where the pause time logic was.
            Interactable = false;
            BlocksRaycasts = false;
            OnStateChangedEvent?.Invoke(State, MenuScreenState.TransitioningIn);
            State = MenuScreenState.TransitioningIn;
            OnWillAppear();
            OnWillAppearEvent?.Invoke();
            if (transition != null)
            {
                transition.Play(MenuTransitionMode.Forward, abInstant).Done(() =>
                {
                    // TBH: This is where the standard resume time logic was.
                    Interactable = true;
                    BlocksRaycasts = true;
                    if (DefaultSelection != null)
                    {
                        MenuHandler.EventSystem.SetSelectedGameObject(DefaultSelection.gameObject);
                    }
                    OnStateChangedEvent?.Invoke(State, MenuScreenState.In);
                    State = MenuScreenState.In;
                    OnDidAppear();
                    OnDidAppearEvent?.Invoke();
                    IPromise promise = transitionInPromise;
                    transitionInPromise = null;
                    promise.Resolve();
                });
            } else
            {
                // TBH: This is where the non-transitioned resume time logic was.
                Interactable = true;
                BlocksRaycasts = true;
                result.Resolve();
            }
            return result;
        }

        /// <summary>
        /// Returns a <see cref="Promise"/> that resolves when this <see cref="MenuScreen"/> finishes transitioning out.
        /// </summary>
        public IPromise TransitionOut(bool abInstant = false)
        {
            if (transitionOutPromise != null)
            {
                return transitionOutPromise;
            }
            transitionOutPromise = Promise.Create();
            IPromise result = transitionOutPromise;
            if (rememberSelection && EventSystem.current.currentSelectedGameObject != null)
            {
                DefaultSelection = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            } else
            {
                DefaultSelection = defaultSelection;
            }
            EventSystem.current.SetSelectedGameObject(null);
            Interactable = false;
            BlocksRaycasts = false;
            OnStateChangedEvent?.Invoke(State, MenuScreenState.TransitioningOut);
            State = MenuScreenState.TransitioningOut;
            OnWillDisappear();
            OnWillDisappearEvent?.Invoke();
            if (transition != null)
            {
                transition.Play(MenuTransitionMode.Reverse, abInstant).Done(() =>
                {
                    gameObject.SetActive(false);
                    OnStateChangedEvent?.Invoke(State, MenuScreenState.Out);
                    State = MenuScreenState.Out;
                    OnDidDisappear();
                    OnDidDisappearEvent?.Invoke();
                    IPromise promise = transitionOutPromise;
                    transitionOutPromise = null;
                    promise.Resolve();
                });
            } else
            {
                result.Resolve();
            }
            return result;
        }
    }
}
