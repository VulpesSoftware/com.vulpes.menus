using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vulpes.Promises;
using Vulpes.Transitions;

namespace Vulpes.Menus
{
    /// <summary>
    /// The <see cref="MenuModal"/> is a verbose popup that can be used to query the user 
    /// and can be displayed with either one, two or three configurable buttons. The resulting 
    /// <see cref="IPromise"/> resolves with a <see cref="MenuModalResult"/> based on the <see cref="Button"/> pressed.
    /// </summary>
    [AddComponentMenu("Vulpes/Menus/Screens/Menu Modal")]
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

        private void SetTextAndToggleGameObject(in string textString, TextMeshProUGUI textMesh, GameObject targetGameObject)
        {
            textMesh.text = textString;
            targetGameObject.SetActive(!string.IsNullOrEmpty(textString));
        }

        private void SetText(in string title, in string body, in string confirm, in string cancel, in string alternate)
        {
            SetTextAndToggleGameObject(title, titleText, titleText.gameObject);
            SetTextAndToggleGameObject(body, bodyText, bodyText.gameObject);
            SetTextAndToggleGameObject(confirm, confirmText, confirmButton.gameObject);
            SetTextAndToggleGameObject(cancel, cancelText, cancelButton.gameObject);
            SetTextAndToggleGameObject(alternate, alternateText, alternateButton.gameObject);
        }

        private void ClearAllButtonListeners()
        {
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            alternateButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Transitions out the <see cref="MenuModal"/> and returns a <see cref="Promise"/> that resolves when the <see cref="Transition"/> is complete.
        /// </summary>
        private IPromise HideInternal()
        {
            ClearAllButtonListeners();
            return MenuHandler.PopScreen(MenuTransitionOptions.InInstant | MenuTransitionOptions.Sequential);
        }

        private void AddListenerForButton(Button button, IPromise<MenuModalResult> promise, MenuModalResult result)
            => button.onClick.AddListener(() => HideInternal().Done(() => promise.Resolve(result)));

        /// <summary>
        /// Transitions in the <see cref="MenuModal"/> and returns a <see cref="Promise"/> that resolves with a <see cref="MenuModalResult"/> when a <see cref="Button"/> is pressed.
        /// </summary>
        private IPromise<MenuModalResult> ShowInternal(in string title, in string body, in string confirm, in string cancel, in string alternate)
        {
            if (IsCurrentScreen)
            {
                Debug.LogWarning("Menu Modal is already in use, you cannot display another Modal until the existing one is dismissed.");
                return Promise<MenuModalResult>.Rejected(null);
            }

            IPromise<MenuModalResult> promise = Promise<MenuModalResult>.Create();

            SetText(title, body, confirm, cancel, alternate);

            ClearAllButtonListeners();

            AddListenerForButton(confirmButton, promise, MenuModalResult.Confirm);
            AddListenerForButton(cancelButton, promise, MenuModalResult.Cancel);
            AddListenerForButton(alternateButton, promise, MenuModalResult.Alternate);

            MenuHandler.PushScreen(this, MenuTransitionOptions.OutInstant);

            return promise;
        }

        /// <summary>
        /// Transitions in a <see cref="MenuModal"/> with one <see cref="Button"/> and returns a <see cref="Promise"/> that resolves with a <see cref="MenuModalResult"/> result when a selection is made.
        /// </summary>
        public IPromise<MenuModalResult> Show(in string title, in string body, in string confirm)
            => ShowInternal(title, body, confirm, null, null);

        /// <summary>
        /// Transitions in a <see cref="MenuModal"/> with two <see cref="Button"/>s and returns a Promise that resolves with a <see cref="MenuModalResult"/> result when a selection is made.
        /// </summary>
        public IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel)
            => ShowInternal(title, body, confirm, cancel, null);

        /// <summary>
        /// Transitions in a <see cref="MenuModal"/> with three <see cref="Button"/>s and returns a <see cref="Promise"/> that resolves with a <see cref="MenuModalResult"/> result when a selection is made.
        /// </summary>
        public IPromise<MenuModalResult> Show(in string title, in string body, in string confirm, in string cancel, in string alternate)
            => ShowInternal(title, body, confirm, cancel, alternate);
    }
}