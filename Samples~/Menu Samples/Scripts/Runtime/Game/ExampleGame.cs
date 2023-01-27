using UnityEngine;
using UnityEngine.SceneManagement;
using Vulpes.Promises;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Game/Example Game")]
    public sealed class ExampleGame : MonoBehaviour
    {
        [SerializeField] private string demoLevel = default;
        [SerializeField] private MenuHandler menuHandler = default;

        private IMenuScreen _pauseScreen;
        private IMenuScreen _gameOverScreen;

        public static ExampleGame Instance { get; private set; }

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _pauseScreen = menuHandler.GetScreen<MenuScreen_Pause>();
            _gameOverScreen = menuHandler.GetScreen<MenuScreen_GameOver>();
        }

        public static IPromise LoadDemoLevel()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(Instance.demoLevel, LoadSceneMode.Additive);
            loadOperation.allowSceneActivation = false;
            return Instance.menuHandler.Loading.Show(
                () => loadOperation.allowSceneActivation = true, 
                () => loadOperation.isDone, 
                () => loadOperation.progress,
                MenuTransitionOptions.Sequential);
        }

        public static IPromise UnloadDemoLevel()
        {
            AsyncOperation loadOperation = null;
            return Instance.menuHandler.Loading.Show(
                () => loadOperation = SceneManager.UnloadSceneAsync(Instance.demoLevel),
                () => loadOperation.isDone,
                () => loadOperation.progress,
                MenuTransitionOptions.Sequential | MenuTransitionOptions.OutInstant);
        }

        public static void Pause()
            => Instance.menuHandler.PushScreen(Instance._pauseScreen, MenuTransitionOptions.Sequential);

        public static void Unpause()
            => Instance.menuHandler.PopScreen(MenuTransitionOptions.Sequential);

        public static void EatCube()
        {
            TastyCube tastyCube = FindObjectOfType<TastyCube>();
            if (tastyCube != null)
            {
                tastyCube.Consume();
                Instance.menuHandler.Alert.Show("Yummy Cube!", null, 3.0f);
                Instance.menuHandler.PushScreen(Instance._gameOverScreen, MenuTransitionOptions.Sequential);
            }
        }
    }
}