using System;
using System.Numerics;
using ClientNetworking;
using GameMenu;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using ObjectComposition;
using View.Factories;

namespace MultiPlayer
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
        private int _createdNumber;

        public MultiplayerPlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, INetworkObjectSender networkObjectSender,
            NotReconciledMovementCommands movementCommands)
        {
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

        public Player CreatePlayer(Vector3 position)
        {
            var playerSimulation = _playerSimulationProvider.CreateSimulationObject();

            IPositionView positionView = playerSimulation.GetView<IPositionView>();
            IForwardAimView forwardAimView = playerSimulation.GetView<IForwardAimView>();
            IHealthView healthView = playerSimulation.GetView<IHealthView>();

            if (_createdNumber == 0)
                positionView = new CompositePositionView(positionView, _cameraView);

            Player player = new Player(
                positionView,
                healthView,
                new ForwardAim(forwardAimView), _bulletFactory, _bulletFactory, position,
                _deathView);

            IMovable movable = player.CharacterMovement;

            if (_createdNumber == 0)
                movable = new MovementCommandSender(player.CharacterMovement, _sender, _notReconciledMovementCommands);

            if (_createdNumber == 0)
            {
                _playerSimulationProvider.InitializeSimulation(playerSimulation, player, movable);
                playerSimulation.Enable();
                _objectToSimulationMapping.RegisterNew(player, playerSimulation);
            }

            _createdNumber++;

            return player;
        }
    }
}