using System;
using System.Collections.Generic;
using System.Linq;
using GameModes.Game;
using GameModes.MultiPlayer.Connection;
using GameModes.MultiPlayer.PlayerCharacter;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using GameModes.Status;
using GameModes.Status.Pause;
using Menus.PauseMenu;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Networking;
using Networking.Connection;
using Networking.ObjectsHashing;
using Networking.PacketReceive;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;
using Networking.PacketSend.ObjectSend;
using Networking.StreamIO;
using Simulation.Infrastructure;
using Simulation.Shooting.Bullets;
using Simulation.SpatialObject;
using UnityEngine;

namespace GameModes.MultiPlayer
{
    public class MultiplayerGame : IGame
    {
        private readonly IGameLoader _gameLoader;
        private readonly UpdatableContainer _updatableContainer = new();
        private LevelConfig _levelConfig;
        private GameStatus _gameStatus;
        private IObjectToSimulationMap _objectToSimulationMap;
        private PlayerClient _clientPlayer;
        private NotReconciledCommands<MoveCommand> _notReconciledMoveCommands;
        private IMovementCommandPrediction _movementCommandPrediction;
        private NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private BulletsContainer _bulletsContainer;
        private ClientServerNetworking _networking;

        public MultiplayerGame(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public async void Load()
        {
            _levelConfig = Resources.Load<LevelConfig>(GameResourceConfigurations.LevelConfigsList);

            var dictionary = new Dictionary<Type, int>().PopulateDictionaryFromTuple(SerializableTypesIdMap.Get());
            ITypeIdConversion typeIdConversion = new TypeIdConversion(dictionary);
            IServerConnectionView connectionView = new ServerConnectionView();
            ClientServerNetworking networking = new ClientServerNetworking(connectionView, typeIdConversion);

            if (await networking.Connect() == false)
                return;

            _networking = networking;
            _networking.StreamRead = new LatencyDebugTestNetworkStreamRead(_networking.StreamRead,
                NetworkConstants.BaseLatency, NetworkConstants.JitterDelta);

            _objectToSimulationMap = new ObjectToSimulationMap();
            _notReconciledMoveCommands = new NotReconciledCommands<MoveCommand>();
            _notReconciledFireCommands = new NotReconciledCommands<FireCommand>();
            _movementCommandPrediction =
                new AllRemotePlayersMovementPrediction(NetworkConstants.RTT, NetworkConstants.ServerFixedDeltaTime);


            PooledBulletFactory bulletFactory =
                BulletFactoryCreation.CreatePooledFactory(_levelConfig.BulletTemplate);

            _bulletsContainer = new BulletsContainer(bulletFactory);

            IPlayerFactory playerFactory = CreatePlayerFactory(_networking.ObjectSender, bulletFactory);
            _clientPlayer = new PlayerClient();

            IPlayerFactory remotePlayerFactory = new RemotePlayerFactory(_levelConfig, bulletFactory,
                _objectToSimulationMap, new NullDeathView(), _updatableContainer, _movementCommandPrediction,
                _bulletsContainer);

            HashedObjectsList hashedObjects = new HashedObjectsList();

            IGenericInterfaceList deserialization = _networking.Deserialization;
            IGenericInterfaceList serialization = _networking.Serialization;
            IGenericInterfaceList receivers = _networking.Receivers;

            deserialization.Register(typeof(ClientPlayer),
                new ClientPlayerSerialization(new PlayerSerialization(hashedObjects, typeIdConversion, playerFactory)));
            deserialization.Register(typeof(Player),
                new PlayerSerialization(hashedObjects, typeIdConversion, remotePlayerFactory));
            deserialization.Register(typeof(MoveCommand),
                new MoveCommandSerialization(hashedObjects, typeIdConversion));
            deserialization.Register(typeof(FireCommand),
                new FireCommandSerialization(hashedObjects, typeIdConversion));

            serialization.Register(typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion));
            serialization.Register(typeof(FireCommand), new FireCommandSerialization(hashedObjects, typeIdConversion));

            receivers.Register(typeof(ClientPlayer), new ClientPlayerReceiver(_objectToSimulationMap, _clientPlayer));
            receivers.Register(typeof(Player), new RemotePlayerReceiver(_objectToSimulationMap));
            receivers.Register(typeof(FireCommand), new FireCommandReceiver(_notReconciledFireCommands, _clientPlayer));
            receivers.Register(typeof(MoveCommand),
                new MoveCommandReceiver(_notReconciledMoveCommands, _clientPlayer, _movementCommandPrediction));
        }

        private IPlayerFactory CreatePlayerFactory(INetworkObjectSender networkObjectSender,
            PooledBulletFactory bulletFactory)
        {
            IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

            GamePause pauseStatus = new GamePause();
            PauseMenu pauseMenu = new PauseMenu(_gameLoader, pauseStatus);
            pauseMenu.Create();
            _gameStatus = new GameStatus(pauseStatus);

            IDeathView death =
                new CompositeDeath(new SetLooseGameStatus(_gameStatus), new OpenMenuOnDeath(_gameLoader));

            IPlayerFactory playerFactory = new ClientPlayerFactory(_levelConfig.PlayerTemplate, cameraView,
                bulletFactory, _objectToSimulationMap, death, networkObjectSender, _notReconciledMoveCommands,
                _notReconciledFireCommands, _bulletsContainer);

            return playerFactory;
        }

        public void UpdateTime(float deltaTime)
        {
            _networking?.ReadNetworkStream();
            _updatableContainer.UpdateTime(deltaTime);
            _bulletsContainer?.Update(deltaTime);
            _clientPlayer?.UpdateTime(deltaTime);
        }
    }
}