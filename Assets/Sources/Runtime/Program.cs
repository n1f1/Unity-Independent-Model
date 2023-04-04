using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClientNetworking;
using ClientNetworking.NetworkingTypesConfigurations;
using GameMenu;
using Model.Characters;
using Networking.ObjectsHashing;
using Networking.Replication.ObjectCreationReplication;
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

        GameUpdate gameUpdate = new GameUpdate();

        //Add callback after built-in Update 
        UnityUpdateReplace unityUpdateReplace = new UnityUpdateReplace();
        unityUpdateReplace.AddAfterUpdate(() => gameUpdate.Update());

        GameLoader gameLoader = new GameLoader(gameUpdate);
        MainMenu mainMenu = new MainMenu(gameLoader);
        mainMenu.Open();

        bool quitting = false;

        Application.quitting += () =>
        {
            unityUpdateReplace.Dispose();
            quitting = true;
        };

        return;

        Game game = new Game();
        game.Start();


        Game.Multiplayer = true;

        if (Game.Multiplayer == false)
        {
            game.CreatePlayerSimulation(new NullObjectSender());
            game.Add(game.PlayerFactory.CreatePlayer(Vector3.Zero));
            game.CreateEnemySpawner();
        }

        if (Game.Multiplayer == false)
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