using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    /// <summary>
    /// Base class for all Menu Screens, contains controls for Time and Cursor Visibility states as well as the Transition type.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup)), DisallowMultipleComponent]
    public abstract class MenuScreen : UIBehaviour, IMenuScreen
    {
        protected CanvasGroup canvasGroup;

        [SerializeField] protected bool showCursor = false;
        [SerializeField] protected Selectable defaultSelection = default;
        [SerializeField] protected MenuTransition transition = default;

        /// <summary>
        /// Called when the Menu Screen will appear (executed after the 'OnWillAppear' method for this screen).
        /// </summary>
        public event Action OnWillAppearEvent;

        /// <summary>
        /// Called when the Menu Screen has finished appearing (executed after the 'OnDidAppear' method for this screen).
        /// </summary>
        public event Action OnDidAppearEvent;

        /// <summary>
        /// Called when the Menu Screen will disappear (executed after the 'OnWillDisappear' method for this screen).
        /// </summary>
        public event Action OnWillDisappearEvent;

        /// <summary>
        /// Called when the Menu Screen has finished disappearing (executed after the 'OnDidDisappear' method for this screen).
        /// </summary>
        public event Action OnDidDisappearEvent;

        /// <summary>
        /// Called when the state of this Menu Screen changes.
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

        public void Initialize(IMenuHandler akMenuHandler)
        {
            MenuHandler = akMenuHandler;
            canvasGroup = GetComponent<CanvasGroup>();
            State = MenuScreenState.Out;
            Interactable = false;
            BlocksRaycasts = false;
            gameObject.SetActive(false);
            OnInitialize();
        }

        /// <summary>
        /// Called when this Menu Screen is first initialized by the Menu Handler.
        /// </summary>
        public virtual void OnInitialize()
        {

        }

        /// <summary>
        /// This method executes every frame that this Menu Screen is the active.
        /// </summary>
        public virtual void OnActive()
        {

        }

        /// <summary>
        /// Called when this Menu Screen begins transitioning in.
        /// </summary>
        public virtual void OnWillAppear()
        {

        }

        /// <summary>
        /// Called when this screen finishes transitioning in.
        /// </summary>
        public virtual void OnDidAppear()
        {

        }

        /// <summary>
        /// Called when this Menu Screen begins transitioning out.
        /// </summary>
        public virtual void OnWillDisappear()
        {

        }

        /// <summary>
        /// Called on when this screen finishes transitioning out.
        /// </summary>
        public virtual void OnDidDisappear()
        {

        }

        /// <summary>
        /// Returns a Promise that resolves when this Menu Screen finishes transitioning in.
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
                    if (defaultSelection != null)
                    {
                        MenuHandler.EventSystem.SetSelectedGameObject(defaultSelection.gameObject);
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
                result.Resolve();
            }
            return result;
        }

        /// <summary>
        /// Returns a Promise that resolves when this Menu Screen finishes transitioning out.
        /// </summary>
        public IPromise TransitionOut(bool abInstant = false)
        {
            if (transitionOutPromise != null)
            {
                return transitionOutPromise;
            }
            transitionOutPromise = Promise.Create();
            IPromise result = transitionOutPromise;
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
                    Interactable = false;
                    BlocksRaycasts = false;
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
