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

        public override void OnDidAppear()
        {
            base.OnDidAppear();
            volumeSlider.OnValueChangedEvent += VolumeSlider_OnValueChangedEvent;
            musicEnumerable.OnValueChangedEvent += MusicEnumerable_OnValueChangedEvent;
        }

        private void VolumeSlider_OnValueChangedEvent(int newValue)
        {
            AudioListener.volume = Mathf.Clamp01(newValue / 100.0f);
        }

        private void MusicEnumerable_OnValueChangedEvent(int newValue)
        {
            Debug.Log("Tell whatever manages music to change to a new track or something...");
        }
    }
}