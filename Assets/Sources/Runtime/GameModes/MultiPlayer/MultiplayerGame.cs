using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameMenu;
using GameMenu.PauseMenu;
using GameModes.MultiPlayer.Connection;
using GameModes.MultiPlayer.NetworkingTypesConfigurations;
using GameModes.MultiPlayer.PlayerCharacter;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.SpatialObject;
using Networking;
using Networking.ObjectsHashing;
using Networking.PacketReceive;
using Networking.PacketReceive.Replication;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;
using Networking.PacketSend.ObjectSend;
using Networking.StreamIO;
using ObjectComposition;
using Simulation.Common;
using Simulation.View;
using Simulation.View.Factories;
using UnityEngine;

namespace GameModes.MultiPlayer
{
    public class MultiplayerGame : IGame
    {
        private readonly IGameLoader _gameLoader;
        private LevelConfig _levelConfig;
        private GameStatus _gameStatus;
        private IObjectToSimulationMap _objectToSimulationMap;
        private PlayerClient _clientPlayer;
        private NotReconciledMovementCommands _notReconciledMovementCommands;
        private UpdatableContainer _updatableContainer = new();
        private IMovementCommandPrediction _movementCommandPrediction;

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
            _notReconciledMovementCommands = new NotReconciledMovementCommands();
            _movementCommandPrediction =
                new AllRemotePlayersMovementPrediction(NetworkConstants.RTT, NetworkConstants.ServerFixedDeltaTime);

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {
                    typeof(MoveCommand),
                    new MoveCommandReceiver(_notReconciledMovementCommands, _clientPlayer, _movementCommandPrediction)
                },
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

            INetworkStreamRead networkStreamRead = new LatencyDebugTestNetworkStreamRead(gameClient,
                NetworkConstants.BaseLatency, NetworkConstants.JitterDelta);

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
                    new OpenMenuOnDeath(_gameLoader)), networkObjectSender, _notReconciledMovementCommands,
                _updatableContainer, _movementCommandPrediction);

            return playerFactory;
        }

        public void UpdateTime(float deltaTime)
        {
            _updatableContainer.UpdateTime(deltaTime);
            _clientPlayer?.UpdateTime(deltaTime);
        }
    }
}