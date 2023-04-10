using System;
using GameModes.MultiPlayer;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation;
using UnityEngine;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.SinglePlayer.ObjectComposition
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly BulletsContainer _bulletsContainer;
        private readonly LevelConfig _levelConfig;

        public PlayerFactory(LevelConfig levelConfig, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, BulletsContainer bulletsContainer)
        {
            _levelConfig = levelConfig;
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _bulletsContainer = bulletsContainer;
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
        }

        public Player CreatePlayer(Vector3 position)
        {
            SinglePlayerTemplate playerTemplate = Object.Instantiate(_levelConfig.PlayerTemplate);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);
            IPlayerView playerView = playerTemplate.PlayerViewBehavior;
            IPlayerSimulation playerSimulation = playerTemplate.PlayerSimulationBehaviour;

            IPositionView positionView = new CompositePositionView(playerView.PositionView, _cameraView);

            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);
            IWeapon weapon = new DefaultGun(_bulletFactory ?? throw new ArgumentException(),
                cooldown,
                _bulletsContainer);

            ForwardAim forwardAim = new ForwardAim(playerView.ForwardAimView);

            Player player = new Player(playerView.HealthView, forwardAim, _deathView,
                weapon, cooldown, new Transform(positionView, position));

            IMovable movable = player.CharacterMovement;

            simulation.AddUpdatableSimulation(playerSimulation.Movable.Initialize(movable));
            simulation.AddUpdatableSimulation(playerSimulation.CharacterShooter.Initialize(player.CharacterShooter));
            simulation.Enable();
            Debug.Log(simulation);

            _objectToSimulationMapping.RegisterNew(player, simulation);

            return player;
        }
    }
}