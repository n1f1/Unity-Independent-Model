using System;
using GameModes.MultiPlayer;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.View.Factories;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.SinglePlayer.ObjectComposition
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly PlayerSimulationProvider _playerSimulationProvider;
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;
        private readonly BulletsContainer _bulletsContainer;

        public PlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping,
            IDeathView deathView, BulletsContainer bulletsContainer)
        {
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _bulletsContainer = bulletsContainer;
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _playerSimulationProvider = new PlayerSimulationProvider(levelConfig.PlayerTemplate, positionViewFactory,
                healthViewFactory);
        }

        public Player CreatePlayer(Vector3 position)
        {
            var playerSimulation = _playerSimulationProvider.CreateSimulationObject();

            IPositionView positionView = playerSimulation.GetView<IPositionView>();
            positionView = new CompositePositionView(positionView, _cameraView);

            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);
            IWeapon weapon = new DefaultGun(_bulletFactory ?? throw new ArgumentException(),
                cooldown,
                _bulletsContainer);

            IHealthView healthView = playerSimulation.GetView<IHealthView>();
            IForwardAimView forwardAimView = playerSimulation.GetView<IForwardAimView>();

            Player player = new Player(healthView, new ForwardAim(forwardAimView), _deathView,
                weapon, cooldown, new Transform(positionView, position));

            IMovable movable = player.CharacterMovement;

            _playerSimulationProvider.InitializeSimulation(playerSimulation, player, movable);

            playerSimulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, playerSimulation);

            return player;
        }
    }
}