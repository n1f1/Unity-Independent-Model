using System.Threading.Tasks;
using Model;
using SceneViewSimulation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.GameLoop;
using View;

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
        IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

        Game game = new Game(new PlayerViewSimulationFactory(gameObjectFactory), cameraView);
        
        //Rip off Update event function from all MonoBehaviours 
        UnityUpdateReplace unityUpdateReplace = new UnityUpdateReplace();
        unityUpdateReplace.Replace(() => game.Update(Time.deltaTime));

        game.Start();
        
        Application.quitting += () => unityUpdateReplace.Dispose();
    }
}