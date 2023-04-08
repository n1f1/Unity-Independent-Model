using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using Model;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Networking;
using Networking.PacketSend.ObjectSend;
using ObjectComposition;
using Simulation;
using Simulation.Common;
using Simulation.View.Factories;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter
{
    internal class MultiplayerPlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly PlayerSimulationProvider _playerSimulationProvider;
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly INetworkObjectSender _sender;
        private readonly NotReconciledMovementCommands _notReconciledMovementCommands;
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private int _createdNumber;
        private UpdatableContainer _updatableContainer;

        public MultiplayerPlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, INetworkObjectSender networkObjectSender,
            NotReconciledMovementCommands movementCommands, UpdatableContainer updatableContainer, IMovementCommandPrediction movementCommandPrediction)
        {
            _movementCommandPrediction = movementCommandPrediction;
            _updatableContainer = updatableContainer;
            _notReconciledMovementCommands =
                movementCommands ?? throw new ArgumentNullException(nameof(movementCommands));
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _playerSimulationProvider = new PlayerSimulationProvider(levelConfig.PlayerTemplate, positionViewFactory,
                healthViewFactory);
            _sender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public Model.Characters.Player CreatePlayer(Vector3 position)
        {
            SimulationObject<Model.Characters.Player> playerSimulation = _playerSimulationProvider.CreateSimulationObject();
            IPositionView positionView = playerSimulation.GetView<IPositionView>();
            IForwardAimView forwardAimView = playerSimulation.GetView<IForwardAimView>();
            IHealthView healthView = playerSimulation.GetView<IHealthView>();

            if (_createdNumber == 0)
                positionView = new CompositePositionView(positionView, _cameraView);

            Model.Characters.Player player = new Model.Characters.Player(
                positionView,
                healthView,
                new ForwardAim(forwardAimView), _bulletFactory, _bulletFactory, position,
                _deathView);

            IMovable movable = player.CharacterMovement;

            if (_createdNumber == 0)
                movable = new ClientPlayerMovementCommandSender(player.CharacterMovement, _sender, _notReconciledMovementCommands);

            if (_createdNumber == 0)
            {
                _playerSimulationProvider.InitializeSimulation(playerSimulation, player, movable);
                playerSimulation.Enable();
                _objectToSimulationMapping.RegisterNew(player, playerSimulation);
            }

            if (_createdNumber != 0)
            {
                ISimulation<RemotePlayerPrediction> simulation =
                    playerSimulation.GameObject.AddComponent<RemotePlayerPredictionSimulation>();

                playerSimulation.AddSimulation(simulation);
                simulation.Initialize(new RemotePlayerPrediction(_movementCommandPrediction,
                    player.CharacterMovement));
                playerSimulation.RegisterUpdatable(simulation);

                playerSimulation.Enable();
                _objectToSimulationMapping.RegisterNew(player, playerSimulation);
                _updatableContainer.QueryAdd(playerSimulation);
            }

            _createdNumber++;

            return player;
        }
    }
}