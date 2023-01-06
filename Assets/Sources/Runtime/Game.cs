using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Common;
using Simulation.Movement;
using Simulation.Physics;
using Simulation.Shooting;
using UnityEngine;
using View;
using View.Factories;

public class Game
{
    private Player _player;
    private Enemy _enemy;
    private readonly UpdatableContainer _updatableContainer = new();

    public void Start()
    {
        LevelConfigsList levelConfigsList = Resources.Load<LevelConfigsList>("LevelConfigsList");
        IPositionView cameraView = Camera.main.GetComponentInParent<PositionView>();

        ISimulationFactory<IMovable> movementFactory = new CharacterMovementFactory();
        ISimulationFactory<CharacterShooter> shooterFactory = new ShooterFactory();

        IViewFactory<IPositionView> positionViewFactory = new PositionViewFactory();
        IViewFactory<IForwardAimView> aimViewFactory = new ForwardAimViewFactory();
        IViewFactory<IHealthView> healthViewFactory = new HealthViewFactory();

        GameObject player = Object.Instantiate(levelConfigsList.PlayerTemplate);
        IPositionView positionView = positionViewFactory.Create(player);
        IHealthView healthView = healthViewFactory.Create(player);
        IAimView aimView = aimViewFactory.Create(player);

        PooledBulletFactory bulletFactory =
            new PooledBulletFactory(positionViewFactory, levelConfigsList.BulletTemplate,
                new SimulatedObjectPool<DefaultBullet>(128), _updatableContainer);

        bulletFactory.PopulatePool();

        _player = new Player(new CompositePositionView(positionView, cameraView), healthView,
            new ForwardAim(aimView), bulletFactory);

        _updatableContainer.QueryAdd(_player);
        _updatableContainer.QueryAdd(movementFactory.Create(_player.CharacterMovement, player));
        _updatableContainer.QueryAdd(shooterFactory.Create(_player.CharacterShooter, player));

        GameObject enemy = Object.Instantiate(levelConfigsList.EnemyTemplate);
        IPositionView enemyPositionView = positionViewFactory.Create(enemy);
        healthViewFactory.Create(enemy);
        Health health = new Health(100f, healthViewFactory.Create(enemy), new Death());
        _enemy = new Enemy(health, enemyPositionView, _player.Transform, _player.Health);
        enemy.AddComponent<DamageablePhysicsInteractableHolder>().Initialize(health);
    }

    public void Update(float deltaTime)
    {
        _updatableContainer.Update(deltaTime);
        _enemy.Update(deltaTime);
    }
}