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
    /// <see cref="Promise"/> resolves with a <see cref="MenuDialogueResult"/> based on the <see cref="Button"/> pressed.
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
        /// Sets the <see cref="TextMeshProUGUI"/> for the <see cref="MenuDialogue"/> and disables any <see cref="Button"/>s with no text string.
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
        /// Transitions in a <see cref="MenuDialogue"/> with one <see cref="Button"/> and returns a <see cref="Promise"/> that resolves with the selected 
        /// <see cref="Button"/> result when the <see cref="MenuTransition"/> is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, Action akOnConfirm = null)
        {
            return ShowInternal(asTitleText, asBodyText, asConfirmText, null, null, akOnConfirm, null, null);
        }

        /// <summary>
        /// Transitions in a <see cref="MenuDialogue"/> with two <see cref="Button"/>s and returns a Promise that resolves with the selected 
        /// <see cref="Button"/> result when the <see cref="MenuTransition"/> is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, Action akOnConfirm = null, Action akOnCancel = null)
        {
            return ShowInternal(asTitleText, asBodyText, asConfirmText, asCancelText, null, akOnConfirm, akOnCancel, null);
        }

        /// <summary>
        /// Transitions in a <see cref="MenuDialogue"/> with three <see cref="Button"/>s and returns a <see cref="Promise"/> that resolves with the selected 
        /// <see cref="Button"/> result when the <see cref="MenuTransition"/> is complete (Note: Callbacks execute before the <see cref="Promise"/> resolves).
        /// </summary>
        public IPromise<MenuDialogueResult> Show(string asTitleText, string asBodyText, string asConfirmText, string asCancelText, string asAlternateText, Action akOnConfirm = null, Action akOnCancel = null, Action akOnAlternate = null)
        {
            return ShowInternal(asTitleText, asBodyText, asConfirmText, asCancelText, asAlternateText, akOnConfirm, akOnCancel, akOnAlternate);
        }

        /// <summary>
        /// Transitions in the <see cref="MenuDialogue"/> and returns a <see cref="Promise"/> that resolves when a <see cref="Button"/> is pressed.
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
        /// Transitions out the <see cref="MenuDialogue"/> and returns a <see cref="Promise"/> that resolves when the <see cref="MenuTransition"/> is complete.
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