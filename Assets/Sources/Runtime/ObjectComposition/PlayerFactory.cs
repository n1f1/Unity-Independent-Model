using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Common;
using SimulationObject;
using UnityEngine;
using View.Factories;

namespace ObjectComposition
{
    internal class PlayerFactory
    {
        private readonly IViewFactory<IPositionView> _positionViewFactory;
        private readonly PlayerSimulationProvider _playerSimulationProvider;
        private readonly LevelConfig _levelConfig;

        public PlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory)
        {
            _levelConfig = levelConfig;
            _positionViewFactory = positionViewFactory;
            _playerSimulationProvider =
                new PlayerSimulationProvider(levelConfig.PlayerTemplate, _positionViewFactory, healthViewFactory);
        }

        public SimulationObject<Player> CreateSimulation() =>
            _playerSimulationProvider.CreateSimulationObject();

        public Player CreatePlayer(SimulationObject<Player> playerSimulation, IPositionView cameraView)
        {
            GameObject bulletTemplate = _levelConfig.BulletTemplate;
            PooledBulletFactory bulletFactory =
                new PooledBulletFactory(new SimulatedSimulationPool<DefaultBullet>(128),
                    new BulletSimulationProvider(bulletTemplate, _positionViewFactory));

            bulletFactory.PopulatePool();

            var player = new Player(new CompositePositionView(playerSimulation.GetView<IPositionView>(), cameraView),
                playerSimulation.GetView<IHealthView>(),
                new ForwardAim(playerSimulation.GetView<IForwardAimView>()), bulletFactory, bulletFactory);

            _playerSimulationProvider.InitializeSimulation(playerSimulation, player);

            return player;
        }
    }
}