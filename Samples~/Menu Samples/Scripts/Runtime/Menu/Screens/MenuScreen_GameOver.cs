using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Game Over")]
    public sealed class MenuScreen_GameOver : MenuScreen
    {
        [SerializeField] private Button quitButton = default;

        private IMenuScreen _titleSceen;

        private void OnQuitButtonPressed()
        {
            ExampleGame.UnloadDemoLevel()
                .Then(() => MenuHandler.SetScreenStack(_titleSceen, MenuTransitionOptions.Sequential));
        }

        public override void OnInitialize()
            => _titleSceen = MenuHandler.GetScreen<MenuScreen_Title>();

        public override void OnDidAppear()
            => quitButton.onClick.AddListener(OnQuitButtonPressed);

        public override void OnWillDisappear()
            => quitButton.onClick.RemoveAllListeners();
    }
}