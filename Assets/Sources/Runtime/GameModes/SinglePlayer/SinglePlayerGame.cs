using System;
using GameModes.Game;
using GameModes.MultiPlayer;
using GameModes.Status;
using GameModes.Status.Pause;
using Menus.PauseMenu;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Simulation;
using Simulation.Characters.EnemyCharacter;
using Simulation.Infrastructure;
using Simulation.Shooting.Bullets;
using Simulation.SpatialObject;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.SinglePlayer
{
    public class SinglePlayerGame : IGame
    {
        private readonly IGameLoader _gameLoader;

        private BulletsContainer _bulletsContainer;
        private SimulationObject _playerSimulation;
        private EnemyContainer _enemyContainer;
        private EnemySpawner _enemySpawner;
        private LevelConfig _levelConfig;
        private GameStatus _gameStatus;
        private Player _player;

        public SinglePlayerGame(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Load()
        {
            _levelConfig = Resources.Load<LevelConfig>(GameResourceConfigurations.LevelConfigsList);
            IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

            GamePause pauseStatus = new GamePause();
            PauseMenu pauseMenu = new PauseMenu(_gameLoader, pauseStatus);
            pauseMenu.Create();
            _gameStatus = new GameStatus(pauseStatus);

            CreatePlayer(cameraView);
            CreateEnemy();
        }

        public void UpdateTime(float deltaTime)
        {
            if (_gameStatus.Finished || _gameStatus.Paused)
                return;

            _bulletsContainer.Update(deltaTime);
            _enemyContainer.UpdateTime(deltaTime);
            _enemySpawner.Update();
            _playerSimulation.UpdateTime(deltaTime);
            _player.UpdateTime(deltaTime);
        }

        private void CreateEnemy()
        {
            IEnemyFactory enemyFactory = new EnemyFactory(_player, _levelConfig.EnemyTemplate);
            _enemyContainer = new EnemyContainer();
            _enemySpawner = new EnemySpawner(3, _enemyContainer, enemyFactory, _player.Transform);
            _enemySpawner.Start();
        }

        private void CreatePlayer(IPositionView cameraView)
        {
            IObjectToSimulationMap objectToSimulationMap = new ObjectToSimulationMap();

            PooledBulletFactory bulletFactory = BulletFactoryCreation.CreatePooledFactory(_levelConfig.BulletTemplate);
            _bulletsContainer = new BulletsContainer(bulletFactory);

            IDeathView playerDeath = new CompositeDeath(
                new SetLooseGameStatus(_gameStatus),
                new OpenMenuOnDeath(_gameLoader));

            SinglePlayerFactory singlePlayerFactory = new SinglePlayerFactory(_levelConfig.PlayerTemplate, cameraView, bulletFactory,
                objectToSimulationMap, playerDeath, _bulletsContainer);

            _player = singlePlayerFactory.CreatePlayer(Vector3.Zero);
            _playerSimulation = objectToSimulationMap.Get(_player);
        }
    }
}