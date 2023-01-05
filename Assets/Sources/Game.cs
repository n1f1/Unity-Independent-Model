using Model;
using Simulation;
using UnityEngine;
using View;

public class Game
{
    private readonly BulletsContainer _bulletsContainer = new();

    private Player _player;
    private Enemy _enemy;
    private readonly UpdatablesContainer _updatablesContainer = new UpdatablesContainer();

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

        PooledBulletFactory bulletFactory =
            new PooledBulletFactory(positionViewFactory, levelConfigsList.BulletTemplate, _bulletsContainer,
                new SimulatedObjectPool<DefaultBullet>(128), _updatablesContainer);
        bulletFactory.PopulatePool();
        
        _player = new Player(new CompositePositionView(positionView, cameraView), healthView,
            new ForwardAim(aimView), bulletFactory);

        _updatablesContainer.Add(movementFactory.Create(_player.CharacterMovement, player));
        _updatablesContainer.Add(shooterFactory.Create(_player.CharacterShooter, player));

        GameObject enemy = Object.Instantiate(levelConfigsList.EnemyTemplate);
        IPositionView enemyPositionView = positionViewFactory.Create(enemy);
        healthViewFactory.Create(enemy);
        Health health = new Health(healthViewFactory.Create(enemy));
        _enemy = new Enemy(health, enemyPositionView, _player.Transform, _player.Health);
        enemy.AddComponent<DamageableCollidable>().Initialize(health);
    }

    public void Update(float deltaTime)
    {
        _updatablesContainer.Update(deltaTime);
        _enemy.Update(deltaTime);
        _bulletsContainer.UpdateBullets(deltaTime);
    }
}