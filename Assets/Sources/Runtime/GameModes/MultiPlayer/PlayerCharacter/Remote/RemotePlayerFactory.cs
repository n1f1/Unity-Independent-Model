using System;
using GameModes.Game;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Simulation;
using Simulation.Infrastructure;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    internal class RemotePlayerFactory : IPlayerFactory
    {
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly UpdatableContainer _updatableContainer;
        private IBulletFactory<IBullet> _bulletFactory;
        private readonly BulletsContainer _bulletsContainer;
        private readonly LevelConfig _levelConfig;

        public RemotePlayerFactory(LevelConfig levelConfig,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, UpdatableContainer updatableContainer,
            IMovementCommandPrediction movementCommandPrediction, BulletsContainer bulletsContainer)
        {
            _levelConfig = levelConfig;
            _movementCommandPrediction = movementCommandPrediction;
            _updatableContainer = updatableContainer;

            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _bulletsContainer = bulletsContainer;
        }

        public Player CreatePlayer(Vector3 position)
        {
            RemotePlayerTemplate playerTemplate = Object.Instantiate(_levelConfig.RemotePlayerTemplate);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);
            IPlayerView playerView = playerTemplate.PlayerView;
            IRemotePlayerSimulation playerSimulation = playerTemplate.RemotePlayerSimulation;

            IPositionView positionView = playerView.PositionView;
            IForwardAimView forwardAimView = playerView.ForwardAimView;

            Transform playerTransform = new Transform(positionView, position);

            forwardAimView = new NullAimView();
            _bulletFactory = new RemoteFiredBulletFactory(playerTransform, _bulletFactory);

            Cooldown cooldown = new Cooldown(0);
            IWeapon weapon = new DefaultGun(_bulletFactory ?? throw new ArgumentException(), cooldown,
                _bulletsContainer);

            Player player = new Player(playerView.HealthView, new ForwardAim(forwardAimView),
                _deathView, weapon, cooldown, playerTransform);


            var prediction =
                new RemotePlayerMovementPrediction(_movementCommandPrediction, player.CharacterMovement);
            simulation.AddUpdatableSimulation(playerSimulation.PlayerMovePrediction.Initialize(prediction));
            _updatableContainer.QueryAdd(simulation);
            _updatableContainer.QueryAdd(player);

            simulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, simulation);

            return player;
        }
    }
}