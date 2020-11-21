using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Play Screen")]
    public sealed class MenuScreen_Play : MenuScreen
    {
        [SerializeField] private TextMeshProUGUI timeText = default;
        [SerializeField] private Button quitButton = default;

        // Don't actually put your gameplay values in the menus, this is just an example.
        // Most Menus should really only convey the information of other systems (i.e. Player Health, Ammo, Keys, etc).
        private float fakeTimer;

        private void UpdateTimerText()
        {
            timeText.text = fakeTimer.ToString("00:00:00");
        }

        public override void OnWillAppear()
        {
            base.OnWillAppear();
            fakeTimer = 0.0f;
            UpdateTimerText();
        }

        public override void OnDidAppear()
        {
            base.OnDidAppear();
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }

        public override void OnWillDisappear()
        {
            base.OnWillDisappear();
            quitButton.onClick.RemoveAllListeners();
        }

        public override void OnActive()
        {
            base.OnActive();
            fakeTimer += Time.deltaTime;
            UpdateTimerText();
        }

        private void OnQuitButtonPressed()
        {
            // Pop the screen and go back to the title.
            MenuHandler.PopScreen();
        }
    }
}