using System;
using System.Threading.Tasks;
using Model.Characters;
using Model.Characters.Enemy;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using ObjectComposition;
using Simulation.Pool;
using SimulationObject;
using SinglePlayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using View;
using View.Factories;
using Vector3 = System.Numerics.Vector3;

namespace GameMenu
{
    public class SinglePlayerGame : IGame
    {
        private readonly IGameLoader _gameLoader;
        private readonly GameStatus _gameStatus = new();

        private LevelConfig _levelConfig;
        private EnemySpawner _enemySpawner;
        private EnemyContainer _enemyContainer;
        private Player _player;
        private SimulationObject<Player> _playerSimulation;

        public SinglePlayerGame(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Load()
        {
            _levelConfig = Resources.Load<LevelConfig>(GameResourceConfigurations.LevelConfigsList);
            IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

            PooledBulletFactory bulletFactory = CreatePooledBulletFactory();

            IObjectToSimulationMap objectToSimulationMap = new ObjectToSimulationMap();
            PlayerFactory playerFactory = new PlayerFactory(_levelConfig, new PositionViewFactory(),
                new HealthViewFactory(), cameraView, bulletFactory, objectToSimulationMap,
                new CompositeDeath(
                    new SetLooseGameStatus(_gameStatus),
                    new OpenMenuOnDeath(_gameLoader)));

            _player = playerFactory.CreatePlayer(Vector3.Zero);
            _playerSimulation = objectToSimulationMap.Get(_player);

            EnemySimulationProvider enemySimulationProvider =
                new EnemySimulationProvider(_levelConfig.EnemyTemplate, new HealthViewFactory(),
                    new PositionViewFactory());

            _enemyContainer = new EnemyContainer();
            _enemySpawner = new EnemySpawner(3, _enemyContainer, new EnemyFactory(_player, enemySimulationProvider),
                _player.Transform);

            _enemySpawner.Start();
        }

        private PooledBulletFactory CreatePooledBulletFactory()
        {
            PooledBulletFactory bulletFactory =
                new PooledBulletFactory(
                    new KeyPooledObjectPool<DefaultBullet, SimulationObject<DefaultBullet>>(64),
                    new BulletSimulationProvider(_levelConfig.BulletTemplate, new PositionViewFactory()));

            bulletFactory.PopulatePool();
            return bulletFactory;
        }

        public void UpdateTime(float deltaTime)
        {
            if (_gameStatus.Finished)
                return;

            _enemyContainer.UpdateTime(deltaTime);
            _enemySpawner.Update();
            _playerSimulation.UpdateTime(deltaTime);
            _player.UpdateTime(deltaTime);
        }
    }
}