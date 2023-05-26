using System;
using System.Collections.Generic;
using GameModes.Game;
using GameModes.MultiPlayer.Connection;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using GameModes.Status;
using GameModes.Status.Pause;
using Menus.PauseMenu;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Networking.Client.Connection;
using Networking.Common;
using Networking.Common.PacketSend.ObjectSend;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Common.Replication.ObjectsHashing;
using Networking.Common.Replication.Serialization;
using Networking.Common.StreamIO.NetworkSimulationTest;
using Simulation.Characters.Player;
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
        private ClientPlayerSimulation _simulationClientPlayer;
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

            GamePause pauseStatus = new GamePause();
            PauseMenu pauseMenu = new PauseMenu(_gameLoader, pauseStatus);
            pauseMenu.Create();
            _gameStatus = new GameStatus(pauseStatus);

            _objectToSimulationMap = new ObjectToSimulationMap();
            _notReconciledMoveCommands = new NotReconciledCommands<MoveCommand>();
            _notReconciledFireCommands = new NotReconciledCommands<FireCommand>();
            _movementCommandPrediction =
                new AllRemotePlayersMovementPrediction(NetworkConstants.RTT, NetworkConstants.ServerFixedDeltaTime);

            PooledBulletFactory bulletFactory =
                BulletFactoryCreation.CreatePooledFactory(_levelConfig.BulletTemplate);
            _bulletsContainer = new BulletsContainer(bulletFactory);

            IPlayerFactory remotePlayerFactory = CreateRemotePlayerFactory(bulletFactory);
            IPlayerFactory playerFactory = CreateClientPlayerFactory(_networking.ObjectSender, bulletFactory);
            _simulationClientPlayer = new ClientPlayerSimulation();

            HashedObjectsList hashedObjects = new HashedObjectsList();
            IGenericInterfaceList deserialization = _networking.Deserialization;
            IGenericInterfaceList serialization = _networking.Serialization;
            IGenericInterfaceList receivers = _networking.Receivers;

            deserialization.Register(typeof(TakeDamageCommand), new TakeDamageCommandSerialization(hashedObjects));
            deserialization.Register(typeof(Player), new PlayerSerialization(hashedObjects, remotePlayerFactory));
            deserialization.Register(typeof(MoveCommand), new MoveCommandSerialization(hashedObjects));
            deserialization.Register(typeof(FireCommand), new FireCommandSerialization(hashedObjects));
            deserialization.Register(typeof(ClientPlayer),
                new ClientPlayerSerialization(new PlayerSerialization(hashedObjects, playerFactory)));

            serialization.Register(typeof(MoveCommand), new MoveCommandSerialization(hashedObjects));
            serialization.Register(typeof(FireCommand), new FireCommandSerialization(hashedObjects));

            receivers.Register(typeof(Player), new RemotePlayerReceiver(_objectToSimulationMap));
            receivers.Register(typeof(TakeDamageCommand), new TakeDamageCommandReceiver());
            receivers.Register(typeof(ClientPlayer),
                new ClientPlayerReceiver(_objectToSimulationMap, _simulationClientPlayer));
            receivers.Register(typeof(FireCommand),
                new FireCommandReceiver(_notReconciledFireCommands, _simulationClientPlayer));
            receivers.Register(typeof(MoveCommand),
                new MoveCommandReceiver(_notReconciledMoveCommands, _simulationClientPlayer,
                    _movementCommandPrediction));
        }

        private IPlayerFactory CreateRemotePlayerFactory(PooledBulletFactory bulletFactory)
        {
            RemotePlayerSimulationInitializer remotePlayerSimulationInitializer =
                new RemotePlayerSimulationInitializer(_objectToSimulationMap, _updatableContainer,
                    _movementCommandPrediction);

            IViewInitializer<IPlayerView> viewInitializer = new ReplaceDeathView(new NullDeathView());
            IPlayerWithViewFactory playerFactory = new RemoteShootingPlayerFactory(bulletFactory, _bulletsContainer);

            RemotePlayerSimulationViewFactory playerSimulationViewFactory =
                new RemotePlayerSimulationViewFactory(_levelConfig.RemotePlayerTemplate);

            IPlayerFactory factory =
                new RemotePlayerFactory(playerSimulationViewFactory,
                    viewInitializer, playerFactory, remotePlayerSimulationInitializer);

            return factory;
        }

        private IPlayerFactory CreateClientPlayerFactory(INetworkObjectSender networkObjectSender,
            PooledBulletFactory bulletFactory)
        {
            IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

            ClientPlayerSimulationViewFactory clientPlayerSimulationViewFactory =
                new ClientPlayerSimulationViewFactory(_levelConfig.PlayerTemplate);

            ClientPlayerSimulationInitializer simulationInitializer = new ClientPlayerSimulationInitializer(
                _objectToSimulationMap, _notReconciledMoveCommands, _notReconciledFireCommands, networkObjectSender);

            IViewInitializer<IPlayerView> viewInitializer = new CompositeViewInitializer<IPlayerView>(
                new ReplaceDeathView(
                    new CompositeDeath(
                        new SetLooseGameStatus(_gameStatus),
                        new OpenMenuOnDeath(_gameLoader))),
                new AddPositionView(cameraView));

            IPlayerWithViewFactory playerFactory = new ShootingPlayerFactory(bulletFactory, _bulletsContainer);

            IPlayerFactory factory =
                new ClientPlayerFactory(clientPlayerSimulationViewFactory,
                    viewInitializer, playerFactory,
                    simulationInitializer);

            return factory;
        }

        public void UpdateTime(float deltaTime)
        {
            _networking?.ReadNetworkStream();
            _updatableContainer.UpdateTime(deltaTime);
            _bulletsContainer?.Update(deltaTime);
            _simulationClientPlayer?.UpdateTime(deltaTime);
        }
    }
}