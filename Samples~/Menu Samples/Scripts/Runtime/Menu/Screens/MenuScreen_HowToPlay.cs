using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/How to Play")]
    public sealed class MenuScreen_HowToPlay : MenuScreen_WithBackButton
    {
        private IMenuScreen _hudScreen;
        private IMenuScreen _optionsScreen;

        [SerializeField] private Button startButton = default;
        [SerializeField] private Button optionsButton = default;

        private void OnStartButtonPressed()
        {
            ExampleGame.LoadDemoLevel()
                .Then(() => MenuHandler.SetScreenStack(_hudScreen, MenuTransitionOptions.Sequential));
        }

        private void OnOptionsButtonPressed()
            => MenuHandler.PushScreen(_optionsScreen, MenuTransitionOptions.Sequential);

        public override void OnDidAppear()
        {
            base.OnDidAppear();
            startButton.onClick.AddListener(OnStartButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        }

        public override void OnWillDisappear()
        {
            base.OnWillDisappear();
            startButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
        }

        public override void OnInitialize()
        {
            _hudScreen = MenuHandler.GetScreen<MenuScreen_HUD>();
            _optionsScreen = MenuHandler.GetScreen<MenuScreen_Options>();
        }

    }
}