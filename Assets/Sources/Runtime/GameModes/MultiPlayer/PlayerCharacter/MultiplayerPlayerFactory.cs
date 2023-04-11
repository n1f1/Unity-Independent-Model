using System;
using GameModes.Game;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Networking.PacketSend.ObjectSend;
using Simulation;
using Simulation.Infrastructure;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter
{
    internal class MultiplayerPlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly INetworkObjectSender _sender;
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly UpdatableContainer _updatableContainer;
        private readonly NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private IBulletFactory<IBullet> _bulletFactory;
        private int _createdNumber;
        private readonly BulletsContainer _bulletsContainer;
        private readonly LevelConfig _levelConfig;

        public MultiplayerPlayerFactory(LevelConfig levelConfig, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, INetworkObjectSender networkObjectSender,
            NotReconciledCommands<MoveCommand> commands, UpdatableContainer updatableContainer,
            IMovementCommandPrediction movementCommandPrediction,
            NotReconciledCommands<FireCommand> notReconciledFireCommands, BulletsContainer bulletsContainer)
        {
            _levelConfig = levelConfig;
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
            _sender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public Player CreatePlayer(Vector3 position)
        {
            RemotePlayerTemplate playerTemplate = Object.Instantiate(_levelConfig.RemotePlayerTemplate);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);
            IPlayerView playerView = playerTemplate.PlayerViewBehavior;
            IRemotePlayerSimulation playerSimulation = playerTemplate.RemotePlayerSimulation;

            IPositionView positionView = playerView.PositionView;
            IForwardAimView forwardAimView = playerView.ForwardAimView;

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

            Player player = new Player(playerView.HealthView, new ForwardAim(forwardAimView),
                _deathView, weapon, cooldown, playerTransform);

            if (_createdNumber == 0)
            {
                IMovable movable = new ClientPlayerMovementCommandSender(player, _sender, _notReconciledCommands);
                var fireCommandSender = new FireCommandSender(player, _sender, _notReconciledFireCommands);
                simulation.AddUpdatableSimulation(playerSimulation.Movable.Initialize(movable));
                simulation.AddUpdatableSimulation(playerSimulation.CharacterShooter.Initialize(fireCommandSender));
            }
            else
            {
                var prediction = new RemotePlayerMovementPrediction(_movementCommandPrediction, player.CharacterMovement);
                simulation.AddUpdatableSimulation(playerSimulation.PlayerMovePrediction.Initialize(prediction));
                _updatableContainer.QueryAdd(simulation);
                _updatableContainer.QueryAdd(player);
            }

            simulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, simulation);
            _createdNumber++;

            return player;
        }
    }
}