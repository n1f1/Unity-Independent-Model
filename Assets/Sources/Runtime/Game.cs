﻿using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using ObjectComposition;
using SimulationObject;
using UnityEngine;
using View;
using View.Factories;

public class Game
{
    private SimulationObject<Player> _playerSimulation;
    private EnemyContainer _enemyContainer;
    private EnemySpawner _enemySpawner;
    private Player _player;
    private Enemy _enemy;

    public void Start()
    {
        LevelConfig levelConfig = Resources.Load<LevelConfig>("LevelConfigsList");
        IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

        IViewFactory<IPositionView> positionViewFactory = new PositionViewFactory();
        IViewFactory<IHealthView> healthViewFactory = new HealthViewFactory();

        PlayerFactory playerFactory = new PlayerFactory(levelConfig, positionViewFactory, healthViewFactory);
        _playerSimulation = playerFactory.CreateSimulation();
        _player = playerFactory.CreatePlayer(_playerSimulation, cameraView);

        _enemyContainer = new EnemyContainer();

        EnemySimulationProvider enemySimulationProvider =
            new EnemySimulationProvider(levelConfig.EnemyTemplate, healthViewFactory, positionViewFactory);

        _enemySpawner = new EnemySpawner(3, _enemyContainer, new EnemyFactory(_player, enemySimulationProvider),
            _player.Transform);
        
        _enemySpawner.Start();
    }

    public void Update(float deltaTime)
    {
        _playerSimulation.UpdateTime(deltaTime);
        _player.UpdateTime(deltaTime);
        _enemyContainer.UpdateTime(deltaTime);
        _enemySpawner.Update();
    }
}