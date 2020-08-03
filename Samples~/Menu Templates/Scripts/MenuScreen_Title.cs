using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vulpes.Menus;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Title Screen")]
    public sealed class MenuScreen_Title : MenuScreen
    {
        [SerializeField] private Button playButton = default;
        [SerializeField] private Button optionsButton = default;
        [SerializeField] private Button quitButton = default;

        // References to other screens that we can navigate to from this screen.
        [SerializeField] private MenuScreen_Options optionsScreen = default;

        /// <summary>
        /// Called when this screen finishes transitioning in.
        /// </summary>
        public override void OnDidAppear()
        {
            base.OnDidAppear();

            // Assign all listeners to the buttons when the screen finishes transitioning in.
            playButton.onClick.AddListener(OnPlayButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }

        /// <summary>
        /// Called when this Menu Screen begins transitioning out.
        /// </summary>
        public override void OnWillDisappear()
        {
            base.OnWillDisappear();

            // Remove all listeners from the buttons when the screen begins transitioning out.
            playButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        private void OnPlayButtonPressed()
        {

        }

        private void OnOptionsButtonPressed()
        {
            // Push the options screen sequentially so that it transitions in after this screen transitions out.
            MenuHandler.PushScreen(optionsScreen, MenuTransitionOptions.Sequential);
        }

        private void OnQuitButtonPressed()
        {
            // Show a popup to see if the user actually wants to quit.
            MenuHandler.Dialogue.Show("Quit?", "Are you sure you want to quit?", "Yes", "No")
                .Then(result =>
                {
                    // Only quit if the user presses the Confirm / Yes button.
                    if (result == MenuDialogueResult.Confirm)
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                })
                .Done();
        }
    }
}