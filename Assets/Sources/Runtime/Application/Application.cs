using System.Threading.Tasks;
using GameModes.Game;
using Menus.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.GameLoop;

namespace Application
{
    public static class Application
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static async void Main()
        {
#if UNITY_EDITOR
            if (SceneManager.GetActiveScene().name.StartsWith("InitTestScene"))
                return;
#elif UNITY_INCLUDE_TESTS
            return;
#endif

#if UNITY_EDITOR
            AsyncOperation sceneLoadingOperation = SceneManager.LoadSceneAsync(0);

            while (!sceneLoadingOperation.isDone)
                await Task.Yield();
#endif

            UnityEngine.Application.targetFrameRate = 60;

            //Add callback after built-in Update 
            GameUpdate gameUpdate = new GameUpdate();
            UnityUpdateReplace unityUpdateReplace = new UnityUpdateReplace();
            unityUpdateReplace.AddAfterUpdate(() => gameUpdate.Update());

            MainMenu mainMenu = new MainMenu(new GameLoader(gameUpdate));
            mainMenu.Open();

            UnityEngine.Application.quitting += () => unityUpdateReplace.Dispose();
        }
    }
}