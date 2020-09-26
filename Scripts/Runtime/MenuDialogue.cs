using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vulpes.Promises;
using System;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuDialogue"/> is a verbose popup that can be used to query the user 
    /// and can be displayed with either one, two or three configurable buttons. The resulting 
    /// <see cref="Promise"/> resolves with a <see cref="MenuDialogueResult"/> based on the button pressed.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Menu Dialogue")]
    public class MenuDialogue : MenuScreen, IMenuDialogue
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
        /// Sets the text for the Dialogue and disables any buttons with no text.
        /// </summary>
        private void SetText(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, string asAlternateText)
        {
            titleText.text = asTitleText;
            titleText.gameObject.SetActive(!string.IsNullOrEmpty(asTitleText));
            bodyText.text = asBodyText;
            bodyText.gameObject.SetActive(!string.IsNullOrEmpty(asBodyText));
            confirmText.text = asConfirmText;
            confirmButton.gameObject.SetActive(!string.IsNullOrEmpty(asConfirmText));
            cancelText.text = asCancelText;
            cancelButton.gameObject.SetActive(!string.IsNullOrEmpty(asCancelText));
            alternateText.text = asAlternateText;
            alternateButton.gameObject.SetActive(!string.IsNullOrEmpty(asAlternateText));
        }

        /// <summary>
        /// Transitions in a Dialogue with one button and returns a <see cref="Promise"/> that resolves with the selected 
        /// button result when the transition is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, Action akOnConfirm = null)
        {
            return ShowInternal(asTitleText, asBodyText, asConfirmText, null, null, akOnConfirm, null, null);
        }

        /// <summary>
        /// Transitions in a Dialogue with two buttons and returns a Promise that resolves with the selected 
        /// button result when the transition is complete (Note: Callbacks execute before the Promise resolves).
        /// </summary>
        public IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, Action akOnConfirm = null, Action akOnCancel = null)
        {
            return ShowInternal(asTitleText, asBodyText, asConfirmText, asCancelText, null, akOnConfirm, akOnCancel, null);
        }

        /// <summary>
        /// Transitions in a Dialogue with three buttons and returns a <see cref="Promise"/> that resolves with the selected 
        /// button result when the transition is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, string asAlternateText, Action akOnConfirm = null, Action akOnCancel = null, Action akOnAlternate = null)
        {
            return ShowInternal(asTitleText, asBodyText, asConfirmText, asCancelText, asAlternateText, akOnConfirm, akOnCancel, akOnAlternate);
        }

        /// <summary>
        /// Transitions in the Dialogue and returns a <see cref="Promise"/> that resolves when a button is pressed.
        /// </summary>
        private IPromise<MenuDialogueResult> ShowInternal(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, string asAlternateText, Action akOnConfirm = null, Action akOnCancel = null, Action akOnAlternate = null)
        {
            if (IsCurrentScreen)
            {
                Debug.LogWarning("Menu Dialogue is already in use, you cannot display another Dialogue until the existing one is dismissed.");
                return Promise<MenuDialogueResult>.Rejected(null);
            }

            IPromise<MenuDialogueResult> promise = Promise<MenuDialogueResult>.Create();

            SetText(asTitleText, asBodyText, asConfirmText, asCancelText, asAlternateText);

            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            alternateButton.onClick.RemoveAllListeners();

            confirmButton.onClick.AddListener(() =>
            {
                HideInternal().Done(() => {
                    akOnConfirm?.Invoke();
                    promise.Resolve(MenuDialogueResult.Confirm);
                });
            });
            cancelButton.onClick.AddListener(() =>
            {
                HideInternal().Done(() => {
                    akOnCancel?.Invoke();
                    promise.Resolve(MenuDialogueResult.Cancel);
                });
            });
            alternateButton.onClick.AddListener(() =>
            {
                HideInternal().Done(() => {
                    akOnAlternate?.Invoke();
                    promise.Resolve(MenuDialogueResult.Alternate);
                });
            });

            MenuHandler.PushScreen(this, MenuTransitionOptions.OutInstant);

            return promise;
        }

        /// <summary>
        /// Transitions out the Dialogue and returns a <see cref="Promise"/> that resolves when the transition is complete.
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