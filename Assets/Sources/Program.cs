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

        LevelConfigsList levelConfigsList = Resources.Load<LevelConfigsList>("LevelConfigsList");
        IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

        Game game = new Game(
            new GameObjectViewSimulationFactory(new GameObjectFactory(levelConfigsList.PlayerTemplate)),
            new GameObjectViewSimulationFactory(new GameObjectFactory(levelConfigsList.EnemyTemplate)), 
            cameraView);

        //Rip off Update event function from all MonoBehaviours 
        UnityUpdateReplace unityUpdateReplace = new UnityUpdateReplace();
        unityUpdateReplace.Replace(() => game.Update(Time.deltaTime));

        game.Start();

        Application.quitting += () => unityUpdateReplace.Dispose();
    }
}