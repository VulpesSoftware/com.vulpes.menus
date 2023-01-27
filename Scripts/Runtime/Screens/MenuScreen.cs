using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vulpes.Promises;
using Vulpes.Transitions;

namespace Vulpes.Menus
{
    public enum MenuScreenCursorVisibilityState : int
    {
        Unchanged,
        ShowCursor,
        HideCursor,
    }

    /// <summary>
    /// Base class for all <see cref="MenuScreen"/>s, contains controls for Cursor Visibility state as well as the Transition type.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup)), DisallowMultipleComponent]
    public abstract class MenuScreen : UIBehaviour, IMenuScreen
    {
        protected CanvasGroup canvasGroup;
        protected GameObject defaultSelection;

        [SerializeField, Tooltip("Cursor visibility state for this screen (leave as 'Unchanged' if you want to handle cursor visibility yourself).")]
        protected MenuScreenCursorVisibilityState cursorVisibility = MenuScreenCursorVisibilityState.Unchanged;

        [SerializeField, Tooltip("If true this screen will remember the previous selection and use it as the default.")] 
        protected bool rememberSelection = false;

        [SerializeField, Tooltip("If assigned this will be the initial selection when this screen appears."), FormerlySerializedAs("defaultSelection")] 
        protected Selectable initialSelection = default;

        [SerializeField, Tooltip("If assigned the screen will use this transition when transitioning.")] 
        protected Transition transition = default;

        private IPromise transitionInPromise;
        private IPromise transitionOutPromise;

        public IMenuHandler MenuHandler { get; protected set; }

        public MenuScreenState State { get; protected set; }

        public bool IsTransitioning => State == MenuScreenState.TransitioningIn || State == MenuScreenState.TransitioningOut;

        public bool IsCurrentScreen
        {
            get
            {
                if (MenuHandler == null)
                {
                    Debug.LogError("No Menu Handler Assigned!");
                    return false;
                }
                return MenuHandler.CurrentScreen != null && MenuHandler.CurrentScreen.Equals(this);
            }
        }

        private void EnableInteraction()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void DisableInteraction()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void ResetDefaultSelection()
            => defaultSelection = initialSelection != null ? initialSelection.gameObject : null;

        private void RememberCurrentSelection()
        {
            if (!rememberSelection)
            {
                return;
            }
            GameObject currentSelection = EventSystem.current.currentSelectedGameObject;
            if (currentSelection != null)
            {
                defaultSelection = currentSelection;
            }
        }

        private void SetCurrentSelectionToDefault()
        {
            if (defaultSelection != null)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelection);
            }
        }

        private void ClearCurrentSelection()
            => EventSystem.current.SetSelectedGameObject(null);

        private void UpdateCursorVisibility()
        {
            switch (cursorVisibility)
            {
                case MenuScreenCursorVisibilityState.ShowCursor:
                    MenuHandler.CursorLocked = false;
                    break;
                case MenuScreenCursorVisibilityState.HideCursor:
                    MenuHandler.CursorLocked = true;
                    break;
                case MenuScreenCursorVisibilityState.Unchanged:
                default:
                    break;
            }
        }

        /// <summary>
        /// Initializes the <see cref="MenuScreen"/>.
        /// </summary>
        public void Initialize(IMenuHandler menuHandler)
        {
            MenuHandler = menuHandler;
            canvasGroup = GetComponent<CanvasGroup>();
            State = MenuScreenState.Out;
            DisableInteraction();
            ResetDefaultSelection();
            gameObject.SetActive(false);
            OnInitialize();
        }

        public void Active()
            => OnActive();

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> is first initialized by the <see cref="Menus.MenuHandler"/>.
        /// </summary>
        public virtual void OnInitialize() { }

        /// <summary>
        /// This method executes every frame that this <see cref="MenuScreen"/> is the active.
        /// </summary>
        public virtual void OnActive() { }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> begins transitioning in.
        /// </summary>
        public virtual void OnWillAppear() { }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> finishes transitioning in.
        /// </summary>
        public virtual void OnDidAppear() { }

        /// <summary>
        /// Called when this <see cref="MenuScreen"/> begins transitioning out.
        /// </summary>
        public virtual void OnWillDisappear() { }

        /// <summary>
        /// Called on when this <see cref="MenuScreen"/> finishes transitioning out.
        /// </summary>
        public virtual void OnDidDisappear() { }

        /// <summary>
        /// Returns a <see cref="Promise"/> that resolves when this <see cref="MenuScreen"/> finishes transitioning in.
        /// </summary>
        public IPromise TransitionIn(MenuScreenTransitionContext context)
        {
            if (transitionInPromise != null)
            {
                return transitionInPromise;
            }
            transitionInPromise = Promise.Create();
            IPromise result = transitionInPromise;
            gameObject.SetActive(true);
            DisableInteraction();
            UpdateCursorVisibility();
            State = MenuScreenState.TransitioningIn;
            OnWillAppear();
            if (transition != null)
            {
                transition.Play(TransitionMode.Forward, context.instant).Done(() =>
                {
                    EnableInteraction();
                    SetCurrentSelectionToDefault();
                    State = MenuScreenState.In;
                    OnDidAppear();
                    IPromise promise = transitionInPromise;
                    transitionInPromise = null;
                    promise.Resolve();
                });
            } else
            {
                EnableInteraction();
                result.Resolve();
            }
            return result;
        }

        /// <summary>
        /// Returns a <see cref="Promise"/> that resolves when this <see cref="MenuScreen"/> finishes transitioning out.
        /// </summary>
        public IPromise TransitionOut(MenuScreenTransitionContext context)
        {
            if (transitionOutPromise != null)
            {
                return transitionOutPromise;
            }
            transitionOutPromise = Promise.Create();
            IPromise result = transitionOutPromise;
            ResetDefaultSelection();
            RememberCurrentSelection();
            ClearCurrentSelection();
            DisableInteraction();
            State = MenuScreenState.TransitioningOut;
            OnWillDisappear();
            if (transition != null)
            {
                transition.Play(TransitionMode.Reverse, context.instant).Done(() =>
                {
                    gameObject.SetActive(false);
                    State = MenuScreenState.Out;
                    OnDidDisappear();
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