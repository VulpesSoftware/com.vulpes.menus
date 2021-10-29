using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vulpes.Menus;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Options Screen")]
    public sealed class MenuScreen_Options : MenuScreen
    {
        [SerializeField] private MenuWidget_Slider volumeSlider = default;
        [SerializeField] private MenuWidget_Enumerable musicEnumerable = default;
        [SerializeField] private Button backButton = default;

        public override void OnDidAppear()
        {
            base.OnDidAppear();
            volumeSlider.onValueChanged.AddListener(VolumeSlider_OnValueChangedEvent);
            musicEnumerable.onValueChanged.AddListener(MusicEnumerable_OnValueChangedEvent);
            backButton.onClick.AddListener(BackButton_OnClick);
        }

        public override void OnWillDisappear()
        {
            base.OnWillDisappear();
            volumeSlider.onValueChanged.RemoveAllListeners();
            musicEnumerable.onValueChanged.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
        }

        private void VolumeSlider_OnValueChangedEvent(int newValue)
        {
            AudioListener.volume = Mathf.Clamp01(newValue / 100.0f);
        }

        private void MusicEnumerable_OnValueChangedEvent(int newValue)
        {
            Debug.Log("Tell whatever manages music to change to a new track or something...");
        }

        private void BackButton_OnClick()
        {
            MenuHandler.PopScreen(MenuTransitionOptions.Sequential);
        }
    }
}