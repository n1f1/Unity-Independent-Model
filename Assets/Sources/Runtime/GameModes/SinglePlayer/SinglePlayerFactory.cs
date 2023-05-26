using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Simulation;
using Simulation.Characters.Player;
using Simulation.Infrastructure;
using Simulation.Shooting;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.SinglePlayer
{
    public class SinglePlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly BulletsContainer _bulletsContainer;
        private readonly SinglePlayerTemplate _playerTemplate;

        public SinglePlayerFactory(SinglePlayerTemplate playerTemplate, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, BulletsContainer bulletsContainer)
        {
            _playerTemplate = playerTemplate ?? throw new ArgumentNullException(nameof(playerTemplate));
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory ?? throw new ArgumentNullException(nameof(pooledBulletFactory));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
        }

        public Player CreatePlayer(Vector3 position)
        {
            SinglePlayerTemplate playerTemplate = Object.Instantiate(_playerTemplate);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);
            IPlayerView playerView = playerTemplate.PlayerView;
            IPlayerSimulation playerSimulation = playerTemplate.PlayerSimulation;

            playerView.PositionView = new CompositePositionView(playerView.PositionView, _cameraView);
            playerView.DeathView = _deathView;

            Player player = CreatePlayer(position, playerView);

            simulation.AddUpdatableSimulation(playerSimulation.Movable.Initialize(player.CharacterMovement));
            simulation.AddUpdatableSimulation(playerSimulation.CharacterShooter.Initialize(player.CharacterCharacterShooter));
            simulation.Enable();

            _objectToSimulationMapping.RegisterNew(player, simulation);

            return player;
        }

        private Player CreatePlayer(Vector3 position, IPlayerView playerView)
        {
            Transform transform = new Transform(playerView.PositionView, position);

            DamageableShooter damageableShooter = new DamageableShooter();

            CharacterShooter characterShooter =
                DefaultPlayer.CreateCharacterShooter(playerView, transform, _bulletFactory, _bulletsContainer,
                    damageableShooter, Player.ShootingCooldown);

            Player player = DefaultPlayer.Player(transform, characterShooter, playerView, damageableShooter);
            damageableShooter.Exclude(player.Damageable);

            return player;
        }
    }
}