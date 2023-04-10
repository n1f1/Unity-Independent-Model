using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer.ObjectComposition;
using Model;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Networking.PacketSend.ObjectSend;
using Simulation;
using Simulation.Common;
using Simulation.Input;
using Simulation.Movement;
using Simulation.Shooting;
using Simulation.View.Factories;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter
{
    internal class MultiplayerPlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly PlayerSimulationProvider _playerSimulationProvider;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly INetworkObjectSender _sender;
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly UpdatableContainer _updatableContainer;
        private readonly NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private IBulletFactory<IBullet> _bulletFactory;
        private int _createdNumber;
        private BulletsContainer _bulletsContainer;

        public MultiplayerPlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, INetworkObjectSender networkObjectSender,
            NotReconciledCommands<MoveCommand> commands, UpdatableContainer updatableContainer,
            IMovementCommandPrediction movementCommandPrediction,
            NotReconciledCommands<FireCommand> notReconciledFireCommands, BulletsContainer bulletsContainer)
        {
            _notReconciledFireCommands = notReconciledFireCommands;
            _movementCommandPrediction = movementCommandPrediction;
            _updatableContainer = updatableContainer;
            _notReconciledCommands =
                commands ?? throw new ArgumentNullException(nameof(commands));
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _bulletsContainer = bulletsContainer;
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _playerSimulationProvider = new PlayerSimulationProvider(levelConfig.PlayerTemplate, positionViewFactory,
                healthViewFactory);
            _sender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public Player CreatePlayer(Vector3 position)
        {
            SimulationObject<Player> playerSimulation = _playerSimulationProvider.CreateSimulationObject();
            IPositionView positionView = playerSimulation.GetView<IPositionView>();
            IForwardAimView forwardAimView = playerSimulation.GetView<IForwardAimView>();
            IHealthView healthView = playerSimulation.GetView<IHealthView>();

            if (_createdNumber == 0)
                positionView = new CompositePositionView(positionView, _cameraView);
            
            Transform playerTransform = new Transform(positionView, position);

            if (_createdNumber != 0)
            {
                forwardAimView = new NullAimView();
                _bulletFactory = new RemoteFiredBulletFactory(playerTransform, _bulletFactory);
            }

            Cooldown cooldown = new Cooldown(_createdNumber == 0 ? Player.ShootingCooldown : 0);
            IWeapon weapon =
                new DefaultGun(_bulletFactory ?? throw new ArgumentException(), cooldown, _bulletsContainer);

            Player player = new Player(healthView, new ForwardAim(forwardAimView),
                _deathView, weapon, cooldown, playerTransform);

            if (_createdNumber == 0)
            {
                IMovable movable = new ClientPlayerMovementCommandSender(player, _sender,
                    _notReconciledCommands);

                playerSimulation.AddSimulation(playerSimulation.GameObject.AddComponent<PlayerMovement>()
                    .Initialize(new AxisInput()));

                ISimulation<IMovable> movableSimulation = playerSimulation.GetSimulation<IMovable>();
                movableSimulation.Initialize(movable);
                playerSimulation.RegisterUpdatable(movableSimulation);

                ISimulation<ICharacterShooter> fireCommandSender =
                    playerSimulation.GameObject.AddComponent<PlayerShooter>();

                playerSimulation.AddSimulation(fireCommandSender);
                fireCommandSender.Initialize(new FireCommandSender(player, _sender, _notReconciledFireCommands));
                playerSimulation.RegisterUpdatable(fireCommandSender);
            }
            else
            {
                ISimulation<RemotePlayerPrediction> simulation =
                    playerSimulation.GameObject.AddComponent<RemotePlayerPredictionSimulation>();

                playerSimulation.AddSimulation(simulation);
                simulation.Initialize(new RemotePlayerPrediction(_movementCommandPrediction,
                    player.CharacterMovement));
                playerSimulation.RegisterUpdatable(simulation);
                _updatableContainer.QueryAdd(playerSimulation);
                _updatableContainer.QueryAdd(player);
            }

            playerSimulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, playerSimulation);
            _createdNumber++;

            return player;
        }
    }
}