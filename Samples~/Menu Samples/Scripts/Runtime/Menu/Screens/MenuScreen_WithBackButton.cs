using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    public abstract class MenuScreen_WithBackButton : MenuScreen
    {
        // This class demonstrates how inheritance can be leveraged to share common
        // functionality between screens. This particular class gives us access 
        // to a back button which is automatically configured to pop the current 
        // screen off of the screen stack.

        [SerializeField] protected Button backButton = default;

        protected virtual void OnBackButtonPressed()
            => MenuHandler.PopScreen(MenuTransitionOptions.Sequential);

        public override void OnDidAppear()
            => backButton.onClick.AddListener(OnBackButtonPressed);

        public override void OnWillDisappear()
            => backButton.onClick.RemoveAllListeners();
    }
}