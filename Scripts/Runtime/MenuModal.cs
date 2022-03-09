using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vulpes.Promises;
using Vulpes.Transitions;
using System;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuModal"/> is a verbose popup that can be used to query the user 
    /// and can be displayed with either one, two or three configurable buttons. The resulting 
    /// <see cref="Promise"/> resolves with a <see cref="MenuModalResult"/> based on the <see cref="Button"/> pressed.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Menu Modal")]
    public class MenuModal : MenuScreen, IMenuModal
    {
        [SerializeField] private TextMeshProUGUI titleText = default;
        [SerializeField] private TextMeshProUGUI bodyText = default;
        [SerializeField] private TextMeshProUGUI confirmText = default;
        [SerializeField] private TextMeshProUGUI cancelText = default;
        [SerializeField] private TextMeshProUGUI alternateText = default;

        [SerializeField] private Button confirmButton = default;
        [SerializeField] private Button cancelButton = default;
        [SerializeField] private Button alternateButton = default;

        /// <summary>
        /// Sets the <see cref="TextMeshProUGUI"/> for the <see cref="MenuModal"/> and disables any <see cref="Button"/>s with no text string.
        /// </summary>
        private void SetText(in string title, in string body, in string confirm, in string cancel, in string alternate)
        {
            titleText.text = title;
            titleText.gameObject.SetActive(!string.IsNullOrEmpty(title));
            bodyText.text = body;
            bodyText.gameObject.SetActive(!string.IsNullOrEmpty(body));
            confirmText.text = confirm;
            confirmButton.gameObject.SetActive(!string.IsNullOrEmpty(confirm));
            cancelText.text = cancel;
            cancelButton.gameObject.SetActive(!string.IsNullOrEmpty(cancel));
            alternateText.text = alternate;
            alternateButton.gameObject.SetActive(!string.IsNullOrEmpty(alternate));
        }

        /// <summary>
        /// Transitions in a <see cref="MenuModal"/> with one <see cref="Button"/> and returns a <see cref="Promise"/> that resolves with a <see cref="MenuModalResult"/>
        /// result when the <see cref="MenuTransition"/> is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, Action onConfirm = null)
        {
            return ShowInternal(title, body, confirm, null, null, onConfirm, null, null);
        }

        /// <summary>
        /// Transitions in a <see cref="MenuModal"/> with two <see cref="Button"/>s and returns a Promise that resolves with a <see cref="MenuModalResult"/>
        /// result when the <see cref="MenuTransition"/> is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel, Action onConfirm = null, Action onCancel = null)
        {
            return ShowInternal(title, body, confirm, cancel, null, onConfirm, onCancel, null);
        }

        /// <summary>
        /// Transitions in a <see cref="MenuModal"/> with three <see cref="Button"/>s and returns a <see cref="Promise"/> that resolves with a <see cref="MenuModalResult"/>
        /// result when the <see cref="MenuTransition"/> is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel, in string alternate, Action onConfirm = null, Action onCancel = null, Action onAlternate = null)
        {
            return ShowInternal(title, body, confirm, cancel, alternate, onConfirm, onCancel, onAlternate);
        }

        /// <summary>
        /// Transitions in the <see cref="MenuModal"/> and returns a <see cref="Promise"/> that resolves with a <see cref="MenuModalResult"/> when a <see cref="Button"/> is pressed.
        /// </summary>
        private IPromise<MenuModalResult> ShowInternal(in string title, in string body, in string confirm, in string cancel, in string alternate, Action onConfirm = null, Action onCancel = null, Action onAlternate = null)
        {
            if (IsCurrentScreen)
            {
                Debug.LogWarning("Menu Dialogue is already in use, you cannot display another Dialogue until the existing one is dismissed.");
                return Promise<MenuModalResult>.Rejected(null);
            }

            IPromise<MenuModalResult> promise = Promise<MenuModalResult>.Create();

            SetText(title, body, confirm, cancel, alternate);

            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            alternateButton.onClick.RemoveAllListeners();

            confirmButton.onClick.AddListener(() =>
            {
                HideInternal().Done(() => {
                    onConfirm?.Invoke();
                    promise.Resolve(MenuModalResult.Confirm);
                });
            });
            cancelButton.onClick.AddListener(() =>
            {
                HideInternal().Done(() => {
                    onCancel?.Invoke();
                    promise.Resolve(MenuModalResult.Cancel);
                });
            });
            alternateButton.onClick.AddListener(() =>
            {
                HideInternal().Done(() => {
                    onAlternate?.Invoke();
                    promise.Resolve(MenuModalResult.Alternate);
                });
            });

            MenuHandler.PushScreen(this, MenuTransitionOptions.OutInstant);

            return promise;
        }

        /// <summary>
        /// Transitions out the <see cref="MenuModal"/> and returns a <see cref="Promise"/> that resolves when the <see cref="Transition"/> is complete.
        /// </summary>
        private IPromise HideInternal()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            alternateButton.onClick.RemoveAllListeners();

            return MenuHandler.PopScreen(MenuTransitionOptions.InInstant | MenuTransitionOptions.Sequential);
        }
    }
}