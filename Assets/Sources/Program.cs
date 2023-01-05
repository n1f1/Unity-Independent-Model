using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.GameLoop;

public static class Program
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

        Game game = new Game();
        game.Start();

        //Rip Update event function out of all MonoBehaviours 
        UnityUpdateReplace unityUpdateReplace = new UnityUpdateReplace();
        unityUpdateReplace.Replace(() => game.Update(Time.deltaTime));

        Application.quitting += () => unityUpdateReplace.Dispose();
    }
}