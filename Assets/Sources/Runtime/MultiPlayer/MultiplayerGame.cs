using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClientNetworking;
using ClientNetworking.NetworkingTypesConfigurations;
using GameMenu;
using GameMenu.PauseMenu;
using Model.Characters;
using Model.SpatialObject;
using MultiPlayer.Connection;
using Networking;
using Networking.ObjectsHashing;
using Networking.Replication;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using ObjectComposition;
using SinglePlayer;
using UnityEngine;
using View;
using View.Factories;

namespace MultiPlayer
{
    public class MultiplayerGame : IGame
    {
        private readonly IGameLoader _gameLoader;
        private LevelConfig _levelConfig;
        private GameStatus _gameStatus;
        private IObjectToSimulationMap _objectToSimulationMap;
        private PlayerClient _clientPlayer;

        public MultiplayerGame(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public async void Load()
        {
            IServerConnectionView connectionView = new ServerConnectionView();
            ServerConnection serverConnection = new ServerConnection(connectionView);

            if (await serverConnection.Connect() == false)
                return;

            NetworkStream networkStream = serverConnection.Client.GetStream();
            IInputStream inputStream = new BinaryReaderInputStream(networkStream);
            IOutputStream outputStream = new BinaryWriterOutputStream(networkStream);


            HashedObjectsList hashedObjects = new HashedObjectsList();

            TypeIdConversion typeIdConversion = new TypeIdConversion(
                new Dictionary<Type, int>().PopulateDictionaryFromTuple(SerializableTypesIdMap.Get()));

            _objectToSimulationMap = new ObjectToSimulationMap();
            _clientPlayer = new PlayerClient();

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {typeof(MoveCommand), new CommandsReceiver()},
                {typeof(Player), new PlayerReceiver(_objectToSimulationMap, _clientPlayer)}
            };

            IEnumerable<(Type, object)> serialization = new List<(Type, object)>
            {
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion))
            };

            INetworkObjectSender objectSender = new StreamObjectSender(serialization, typeIdConversion, outputStream);
            IPlayerFactory playerFactory = CreatePlayerFactory(objectSender);

            IEnumerable<(Type, object)> deserialization = new List<(Type, object)>
            {
                (typeof(Player), new PlayerSerialization(hashedObjects, typeIdConversion, playerFactory)),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion))
            };

            IReplicationPacketRead packetRead =
                new SendToReceiversPacketRead(receivers, deserialization, typeIdConversion);
            
            GameClient gameClient = new GameClient(packetRead);
            INetworkStreamRead networkStreamRead = new LatencyDebugTestNetworkStreamRead(gameClient);

            bool quitting = false;
            Application.quitting += () => quitting = true;

            while (quitting == false)
            {
                networkStreamRead.ReadNetworkStream(inputStream);
                await Task.Yield();
            }
        }

        private IPlayerFactory CreatePlayerFactory(INetworkObjectSender networkObjectSender)
        {
            _levelConfig = Resources.Load<LevelConfig>(GameResourceConfigurations.LevelConfigsList);
            IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

            GamePause pauseStatus = new GamePause();
            PauseMenu pauseMenu = new PauseMenu(_gameLoader, pauseStatus);
            pauseMenu.Create();
            _gameStatus = new GameStatus(pauseStatus);

            PooledBulletFactory bulletFactory =
                BulletFactoryCreation.CreatePooledBulletFactory(_levelConfig.BulletTemplate);

            IPlayerFactory playerFactory = new MultiplayerPlayerFactory(_levelConfig, new PositionViewFactory(),
                new HealthViewFactory(), cameraView, bulletFactory, _objectToSimulationMap,
                new CompositeDeath(
                    new SetLooseGameStatus(_gameStatus),
                    new OpenMenuOnDeath(_gameLoader)), networkObjectSender);

            return playerFactory;
        }

        public void UpdateTime(float deltaTime)
        {
            _clientPlayer?.UpdateTime(deltaTime);
        }
    }
}