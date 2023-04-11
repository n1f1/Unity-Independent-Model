using System;
using GameModes.Game;
using GameModes.MultiPlayer.PlayerCharacter.Client.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using GameModes.SinglePlayer;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Networking.PacketSend.ObjectSend;
using Simulation;
using Simulation.Characters.Player;
using Simulation.Infrastructure;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
{
    internal class ClientPlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly INetworkObjectSender _sender;
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly BulletsContainer _bulletsContainer;
        private readonly LevelConfig _levelConfig;
        private readonly SinglePlayerTemplate _playerTemplate;

        public ClientPlayerFactory(SinglePlayerTemplate playerTemplate,
            IPositionView cameraView, IBulletFactory<IBullet> pooledBulletFactory,
            IObjectToSimulationMap objectToSimulationMapping, IDeathView deathView,
            INetworkObjectSender networkObjectSender,
            NotReconciledCommands<MoveCommand> commands, NotReconciledCommands<FireCommand> notReconciledFireCommands,
            BulletsContainer bulletsContainer)
        {
            _playerTemplate = playerTemplate;
            _notReconciledFireCommands = notReconciledFireCommands;
            _notReconciledCommands =
                commands ?? throw new ArgumentNullException(nameof(commands));
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _bulletsContainer = bulletsContainer;
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _sender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public Player CreatePlayer(Vector3 position)
        {
            SinglePlayerTemplate playerTemplate = Object.Instantiate(_playerTemplate);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);
            IPlayerView playerView = playerTemplate.PlayerView;
            IPlayerSimulation playerSimulation = playerTemplate.PlayerSimulation;

            IPositionView positionView = playerView.PositionView;
            IForwardAimView forwardAimView = playerView.ForwardAimView;

            positionView = new CompositePositionView(positionView, _cameraView);

            Transform playerTransform = new Transform(positionView, position);

            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);
            IWeapon weapon =
                new DefaultGun(_bulletFactory ?? throw new ArgumentException(), cooldown, _bulletsContainer);

            Player player = new Player(playerView.HealthView, new ForwardAim(forwardAimView),
                _deathView, weapon, cooldown, playerTransform);

            IMovable movable = new ClientPlayerMovementCommandSender(player, _sender, _notReconciledCommands);
            var fireCommandSender = new FireCommandSender(player, _sender, _notReconciledFireCommands);
            simulation.AddUpdatableSimulation(playerSimulation.Movable.Initialize(movable));
            simulation.AddUpdatableSimulation(playerSimulation.CharacterShooter.Initialize(fireCommandSender));

            simulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, simulation);

            return player;
        }
    }
}