using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/HUD")]
    public sealed class MenuScreen_HUD : MenuScreen
    {
        [SerializeField] private Button pauseButton = default;
        [SerializeField] private Button eatCubeButton = default;

        private void OnPauseButtonPressed()
            => ExampleGame.Pause();

        private void OnEatCubeButtonPressed()
            => ExampleGame.EatCube();

        public override void OnDidAppear()
        {
            pauseButton.onClick.AddListener(OnPauseButtonPressed);
            eatCubeButton.onClick.AddListener(OnEatCubeButtonPressed);
        }

        public override void OnWillDisappear()
        {
            pauseButton.onClick.RemoveAllListeners();
            eatCubeButton.onClick.RemoveAllListeners();
        }
    }
}