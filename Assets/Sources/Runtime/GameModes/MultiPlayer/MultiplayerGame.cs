using System;
using System.Collections.Generic;
using System.Net.Sockets;
using GameMenu;
using GameMenu.PauseMenu;
using GameModes.MultiPlayer.Connection;
using GameModes.MultiPlayer.NetworkingTypesConfigurations;
using GameModes.MultiPlayer.PlayerCharacter;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer;
using GameModes.SinglePlayer.ObjectComposition;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Networking.ObjectsHashing;
using Networking.PacketReceive;
using Networking.PacketReceive.Replication;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;
using Networking.PacketSend.ObjectSend;
using Networking.StreamIO;
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
        private NotReconciledCommands<MoveCommand> _notReconciledMoveCommands;
        private readonly UpdatableContainer _updatableContainer = new();
        private IMovementCommandPrediction _movementCommandPrediction;
        private NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private INetworkStreamRead _networkStreamRead;
        private IInputStream _inputStream;
        private BulletsContainer _bulletsContainer;

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
            IOutputStream outputStream = new BinaryWriterOutputStream(networkStream);
            _inputStream = new BinaryReaderInputStream(networkStream);

            HashedObjectsList hashedObjects = new HashedObjectsList();

            TypeIdConversion typeIdConversion = new TypeIdConversion(
                new Dictionary<Type, int>().PopulateDictionaryFromTuple(SerializableTypesIdMap.Get()));

            _objectToSimulationMap = new ObjectToSimulationMap();
            _clientPlayer = new PlayerClient();
            _notReconciledMoveCommands = new NotReconciledCommands<MoveCommand>();
            _notReconciledFireCommands = new NotReconciledCommands<FireCommand>();
            _movementCommandPrediction =
                new AllRemotePlayersMovementPrediction(NetworkConstants.RTT, NetworkConstants.ServerFixedDeltaTime);

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {
                    typeof(MoveCommand),
                    new MoveCommandReceiver(_notReconciledMoveCommands, _clientPlayer, _movementCommandPrediction)
                },
                {typeof(Player), new PlayerReceiver(_objectToSimulationMap, _clientPlayer)},
                {typeof(FireCommand), new FireCommandReceiver(_notReconciledFireCommands, _clientPlayer)}
            };

            IEnumerable<(Type, object)> serialization = new List<(Type, object)>
            {
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion)),
                (typeof(FireCommand), new FireCommandSerialization(hashedObjects, typeIdConversion))
            };

            INetworkObjectSender objectSender = new StreamObjectSender(serialization, typeIdConversion, outputStream);
            IPlayerFactory playerFactory = CreatePlayerFactory(objectSender);

            IEnumerable<(Type, object)> deserialization = new List<(Type, object)>
            {
                (typeof(Player), new PlayerSerialization(hashedObjects, typeIdConversion, playerFactory)),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion)),
                (typeof(FireCommand), new FireCommandSerialization(hashedObjects, typeIdConversion))
            };

            IReplicationPacketRead packetRead =
                new SendToReceiversPacketRead(receivers, deserialization, typeIdConversion);

            GameClient gameClient = new GameClient(packetRead);

            _networkStreamRead = new LatencyDebugTestNetworkStreamRead(gameClient,
                NetworkConstants.BaseLatency, NetworkConstants.JitterDelta);
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

            _bulletsContainer = new BulletsContainer(bulletFactory);
            
            IPlayerFactory playerFactory = new MultiplayerPlayerFactory(_levelConfig, new PositionViewFactory(),
                new HealthViewFactory(), cameraView, bulletFactory, _objectToSimulationMap,
                new CompositeDeath(
                    new SetLooseGameStatus(_gameStatus),
                    new OpenMenuOnDeath(_gameLoader)), networkObjectSender, _notReconciledMoveCommands,
                _updatableContainer, _movementCommandPrediction, _notReconciledFireCommands, _bulletsContainer);

            return playerFactory;
        }

        public void UpdateTime(float deltaTime)
        {
            _networkStreamRead?.ReadNetworkStream(_inputStream);
            _updatableContainer.UpdateTime(deltaTime);
            _bulletsContainer?.Update(deltaTime);
            _clientPlayer?.UpdateTime(deltaTime);
        }
    }
}