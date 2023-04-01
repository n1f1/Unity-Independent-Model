using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClientNetworking;
using ClientNetworking.NetworkingTypesConfigurations;
using Model.Characters;
using Networking.ObjectsHashing;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using ObjectComposition;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.GameLoop;
using Vector3 = System.Numerics.Vector3;

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

        Application.targetFrameRate = 60;
        Game game = new Game();
        
        Game.Multiplayer = false;
        game.Start();
        game.CreatePlayerSimulation(new NullObjectSender());
        game.Add(game.PlayerFactory.CreatePlayer(Vector3.Zero));
        game.CreateEnemySpawner();

        //Rip Update event function out of all MonoBehaviours 
        UnityUpdateReplace unityUpdateReplace = new UnityUpdateReplace();
        unityUpdateReplace.Replace(() => game.Update(Time.deltaTime));

        bool quitting = false;

        Application.quitting += () =>
        {
            unityUpdateReplace.Dispose();
            quitting = true;
        };

        if(true)
            return;
        
        using TcpClient tcpClient = new TcpClient();

        await tcpClient.ConnectAsync("192.168.1.87", 55555);

        Debug.Log("connect");

        NetworkStream networkStream = tcpClient.GetStream();
        IInputStream inputStream = new BinaryReaderInputStream(networkStream);
        IOutputStream outputStream = new BinaryWriterOutputStream(networkStream);

        GameClient gameClient = new GameClient(inputStream, outputStream);

        HashedObjectsList hashedObjects = new HashedObjectsList();

        TypeIdConversion typeIdConversion = new TypeIdConversion(
            new Dictionary<Type, int>().PopulateDictionaryFromTuple(SerializableTypesIdMap.Get()));

        Dictionary<Type, object> dictionary = new Dictionary<Type, object>
        {
            {typeof(MoveCommand), new CommandsReceiver()},
            {typeof(Player), new PlayerReceiver(game)}
        };

        IEnumerable<(Type, object)> serialization = new List<(Type, object)>
        {
            (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion))
        };
        gameClient.CreateNetworkSending(serialization, typeIdConversion);
        game.CreatePlayerSimulation(GameClient.ObjectSender);

        IEnumerable<(Type, object)> deserialization = new List<(Type, object)>
        {
            (typeof(Player), new PlayerSerialization(hashedObjects, typeIdConversion, game.PlayerFactory)),
            (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion))
        };
        gameClient.CreateReplicator(dictionary, deserialization, typeIdConversion);

        while (quitting == false)
        {
            gameClient.ReceivePackets();
            await Task.Yield();
        }
    }
}