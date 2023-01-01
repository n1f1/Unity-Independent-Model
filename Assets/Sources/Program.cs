using System.Threading.Tasks;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        GameObjectFactory<Player> gameObjectFactory = Object.FindObjectOfType<GameObjectFactory<Player>>();
        Game game = new Game(new PlayerViewSimulationFactory(gameObjectFactory));
        game.Start();
        gameObjectFactory.Add(() => game.Update(Time.deltaTime));
    }
}