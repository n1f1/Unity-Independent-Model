using System;
using GameMenu;
using GameModes;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.View.Factories;
using Vector3 = System.Numerics.Vector3;

namespace ObjectComposition
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IPositionView _cameraView;
        private readonly PlayerSimulationProvider _playerSimulationProvider;
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IDeathView _deathView;

        public PlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory, IPositionView cameraView,
            IBulletFactory<IBullet> pooledBulletFactory, IObjectToSimulationMap objectToSimulationMapping, IDeathView deathView)
        {
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
            _bulletFactory = pooledBulletFactory;
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _playerSimulationProvider = new PlayerSimulationProvider(levelConfig.PlayerTemplate, positionViewFactory,
                healthViewFactory);
        }

        public Player CreatePlayer(Vector3 position)
        {
            var playerSimulation = _playerSimulationProvider.CreateSimulationObject();

            IPositionView positionView = playerSimulation.GetView<IPositionView>();
            positionView = new CompositePositionView(positionView, _cameraView);

            Player player = new Player(
                positionView,
                playerSimulation.GetView<IHealthView>(),
                new ForwardAim(playerSimulation.GetView<IForwardAimView>()), _bulletFactory, _bulletFactory, position,
                _deathView);

            IMovable movable = player.CharacterMovement;
            
            _playerSimulationProvider.InitializeSimulation(playerSimulation, player, movable);

            playerSimulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, playerSimulation);

            return player;
        }
    }
}