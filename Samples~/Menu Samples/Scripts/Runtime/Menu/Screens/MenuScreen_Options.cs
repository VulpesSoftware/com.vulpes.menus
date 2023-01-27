using System;
using UnityEngine;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Options")]
    public sealed class MenuScreen_Options : MenuScreen_WithBackButton
    {
        [SerializeField] private MenuWidget_Enumerable cubeShapeEnumerable = default;
        [SerializeField] private MenuWidget_Slider volumeSlider = default;
        [SerializeField] private MenuWidget_Toggle seaShantiesToggle = default;
        [SerializeField] private MenuWidget_Slider menuOpacitySlider = default;
        [SerializeField] private MenuWidget_Button whatTimeIsItButton = default;

        [SerializeField] private CanvasGroup rootLevelCanvasGroup = default;

        private void OnCubeShapeEnumerableValueChanged(int newValue)
            => Debug.Log($"Cube Shape Enumerable = {newValue}");

        private void OnVolumeSliderValueChanged(int newValue)
            => AudioListener.volume = (float)newValue / volumeSlider.MaxValue;

        private void OnSeaShantiesToggleValueChanged(bool newValue)
            => Debug.Log($"Sea Shanties Toggle = {newValue}");

        private void OnMenuOpacitySliderValueChanged(int newValue)
            => rootLevelCanvasGroup.alpha = (float)newValue / menuOpacitySlider.MaxValue;

        private void OnWhatTimeIsItButtonPressed()
            => MenuHandler.Alert.Show($"The current time is: {DateTime.Now:hh:mm:ss tt}", null, 5.0f);

        public override void OnDidAppear()
        {
            base.OnDidAppear();
            cubeShapeEnumerable.onValueChanged.AddListener(OnCubeShapeEnumerableValueChanged);
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChanged);
            seaShantiesToggle.onValueChanged.AddListener(OnSeaShantiesToggleValueChanged);
            menuOpacitySlider.onValueChanged.AddListener(OnMenuOpacitySliderValueChanged);
            whatTimeIsItButton.onClick.AddListener(OnWhatTimeIsItButtonPressed);
        }

        public override void OnWillDisappear()
        {
            base.OnWillDisappear();
            cubeShapeEnumerable.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.RemoveAllListeners();
            seaShantiesToggle.onValueChanged.RemoveAllListeners();
            menuOpacitySlider.onValueChanged.RemoveAllListeners();
            whatTimeIsItButton.onClick.RemoveAllListeners();
        }
    }
}