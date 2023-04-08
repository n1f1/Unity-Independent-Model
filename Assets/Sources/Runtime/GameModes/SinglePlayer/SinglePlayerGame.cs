using System;
using GameMenu;
using GameMenu.PauseMenu;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using ObjectComposition;
using Simulation;
using Simulation.View;
using Simulation.View.Factories;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.SinglePlayer
{
    public class SinglePlayerGame : IGame
    {
        private readonly IGameLoader _gameLoader;

        private GameStatus _gameStatus;
        private LevelConfig _levelConfig;
        private EnemySpawner _enemySpawner;
        private EnemyContainer _enemyContainer;
        private Player _player;
        private SimulationObject<Player> _playerSimulation;
        private readonly BulletFactoryCreation _bulletFactoryCreation;

        public SinglePlayerGame(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
            _bulletFactoryCreation = new BulletFactoryCreation();
        }

        public void Load()
        {
            _levelConfig = Resources.Load<LevelConfig>(GameResourceConfigurations.LevelConfigsList);
            IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

            GamePause pauseStatus = new GamePause();
            PauseMenu pauseMenu = new PauseMenu(_gameLoader, pauseStatus);
            pauseMenu.Create();
            _gameStatus = new GameStatus(pauseStatus);

            PooledBulletFactory bulletFactory =
                BulletFactoryCreation.CreatePooledBulletFactory(_levelConfig.BulletTemplate);

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

        public void UpdateTime(float deltaTime)
        {
            if (_gameStatus.Finished || _gameStatus.Paused)
                return;

            _enemyContainer.UpdateTime(deltaTime);
            _enemySpawner.Update();
            _playerSimulation.UpdateTime(deltaTime);
            _player.UpdateTime(deltaTime);
        }
    }
}