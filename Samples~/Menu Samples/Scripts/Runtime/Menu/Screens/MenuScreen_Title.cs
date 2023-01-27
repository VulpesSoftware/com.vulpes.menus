using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Title")]
    public sealed class MenuScreen_Title : MenuScreen
    {
        // You can of course serialize your screens and assign them in the inspector, but for the sake of 
        // simplicity we'll just make them private and cache them in 'OnInitialize'.
        private IMenuScreen _howToPlayScreen;
        private IMenuScreen _optionsScreen;

        [SerializeField] private Button playButton = default;
        [SerializeField] private Button optionsButton = default;
        [SerializeField] private Button quitButton = default;

        private void OnPlayButtonPressed()
            => MenuHandler.PushScreen(_howToPlayScreen, MenuTransitionOptions.Sequential);

        private void OnOptionsButtonPressed()
            => MenuHandler.PushScreen(_optionsScreen, MenuTransitionOptions.Sequential);

        private void OnQuitButtonPressed()
        {
            // Here we show a generic modal window to ask the user to confirm that they want to quit.
            // When a selection is made, the Show Promise will resolve with a return type of 'MenuModalResult',
            // we can use this to execute specific code according to our choice.
            MenuHandler.Modal.Show("QUIT?", "Are you sure you want to quit?", "QUIT", "CANCEL")
                .Then(result =>
                {
                    // We only care about the confirm option here, all other selections will automatically pop 
                    // the modal off of the screen stack.
                    if (result == MenuModalResult.Confirm)
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                });
        }

        public override void OnInitialize()
        {
            // OnInitialize is only called once, so this is the best place to cache any references for this screen.
            _howToPlayScreen = MenuHandler.GetScreen<MenuScreen_HowToPlay>();
            _optionsScreen = MenuHandler.GetScreen<MenuScreen_Options>();
        }

        public override void OnDidAppear()
        {
            // It's best practice to assign any UI event listeners after the screen has finished transitioning in.
            playButton.onClick.AddListener(OnPlayButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }

        public override void OnWillDisappear()
        {
            // Similarly it's a good idea to unassign all listeners prior to transitioning out.
            playButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}