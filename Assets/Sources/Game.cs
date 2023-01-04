using System.Collections.Generic;
using Model;
using Simulation;
using UnityEngine;
using View;

public class Game
{
    private readonly List<IUpdatable> _simulations = new();
    private readonly BulletsContainer _bulletsContainer = new();

    private Player _player;
    private Enemy _enemy;

    public void Start()
    {
        LevelConfigsList levelConfigsList = Resources.Load<LevelConfigsList>("LevelConfigsList");
        IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

        ISimulationFactory<PlayerMovement, IMovable> movementFactory = new CharacterMovementFactory();
        ISimulationFactory<PlayerShooter, CharacterShooter> shooterFactory = new ShooterFactory();
        
        IViewFactory<IPositionView> positionViewFactory = new PositionViewFactory();
        IViewFactory<IForwardAimView> aimViewFactory = new ForwardAimViewFactory();
        IViewFactory<IHealthView> healthViewFactory = new HealthViewFactory();
        
        GameObject player = Object.Instantiate(levelConfigsList.PlayerTemplate);
        IPositionView positionView = positionViewFactory.Create(player);
        IHealthView healthView = healthViewFactory.Create(player);
        IAimView aimView = aimViewFactory.Create(player);
        
        _player = new Player(new CompositePositionView(positionView, cameraView), healthView,
            new ForwardAim(aimView), new BulletFactory(positionViewFactory, levelConfigsList.BulletTemplate, _bulletsContainer));

        _simulations.Add(movementFactory.Create(_player.CharacterMovement, player));
        _simulations.Add(shooterFactory.Create(_player.CharacterShooter, player));

        GameObject enemy = Object.Instantiate(levelConfigsList.EnemyTemplate);
        IPositionView enemyPositionView = positionViewFactory.Create(enemy);
        _enemy = new Enemy(enemyPositionView, _player.Transform, _player.Health);
    }
    
    public void Update(float deltaTime)
    {
        foreach (IUpdatable simulation in _simulations) 
            simulation.UpdatePassedTime(deltaTime);

        _enemy.Update(deltaTime);
        _bulletsContainer.UpdateBullets(deltaTime);
    }
}