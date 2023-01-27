using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Screens/Pause")]
    public sealed class MenuScreen_Pause : MenuScreen
    {
        private IMenuScreen _titleSceen;
        private IMenuScreen _optionsSceen;

        [SerializeField] private Button resumeButton = default;
        [SerializeField] private Button optionsButton = default;
        [SerializeField] private Button quitButton = default;

        private void OnResumeButtonPressed()
            => ExampleGame.Unpause();

        private void OnOptionsButtonPressed()
            => MenuHandler.PushScreen(_optionsSceen, MenuTransitionOptions.Sequential);

        private void OnQuitButtonPressed()
        {
            MenuHandler.Modal.Show("QUIT?", "Are you sure you want to quit?", "TO TITLE", "TO DESKTOP", "CANCEL")
                .Then(result =>
                {
                    switch (result)
                    {
                        case MenuModalResult.Confirm:
                            ExampleGame.UnloadDemoLevel()
                                .Then(() => MenuHandler.SetScreenStack(_titleSceen, MenuTransitionOptions.Sequential));
                            break;
                        case MenuModalResult.Cancel:
#if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                            break;
                        case MenuModalResult.Alternate:
                            break;
                    }
                });
        }

        public override void OnInitialize()
        {
            _titleSceen = MenuHandler.GetScreen<MenuScreen_Title>();
            _optionsSceen = MenuHandler.GetScreen<MenuScreen_Options>();
        }

        public override void OnDidAppear()
        {
            base.OnDidAppear();
            resumeButton.onClick.AddListener(OnResumeButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }

        public override void OnWillDisappear()
        {
            base.OnWillDisappear();
            resumeButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}