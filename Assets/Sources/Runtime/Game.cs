using Model;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Common;
using Simulation.Movement;
using Simulation.Shooting;
using UnityEngine;
using View;
using View.Factories;
using Object = UnityEngine.Object;

public class Game
{
    private Player _player;
    private Enemy _enemy;
    private readonly UpdatableContainer _updatableContainer = new();
    private EnemyContainer _enemyContainer;
    private EnemySpawner _enemySpawner;

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
        SimulationObject<Player> simulation = new SimulationObject<Player>(player);
        simulation.Add(positionViewFactory.Create(player));
        simulation.Add(healthViewFactory.Create(player));
        simulation.Add(aimViewFactory.Create(player));

        PooledBulletFactory bulletFactory =
            new PooledBulletFactory(positionViewFactory, levelConfigsList.BulletTemplate,
                new SimulatedSimulationPool<DefaultBullet>(128));

        bulletFactory.PopulatePool();

        _player = new Player(new CompositePositionView(simulation.GetView<IPositionView>(), cameraView),
            simulation.GetView<IHealthView>(),
            new ForwardAim(simulation.GetView<IForwardAimView>()), bulletFactory, bulletFactory);

        simulation.AddSimulation(movementFactory.Create(_player.CharacterMovement, player));
        simulation.AddSimulation(shooterFactory.Create(_player.CharacterShooter, player));

        ISimulation<IMovable> movableSimulation = simulation.GetSimulation<IMovable>();
        movableSimulation.Initialize(_player.CharacterMovement);
        ISimulation<CharacterShooter> characterShooter = simulation.GetSimulation<CharacterShooter>();
        characterShooter.Initialize(_player.CharacterShooter);
        
        _updatableContainer.QueryAdd(_player);
        _updatableContainer.QueryAdd(movableSimulation);
        _updatableContainer.QueryAdd(characterShooter);

        _enemyContainer = new EnemyContainer();
        EnemySpawner spawner = new EnemySpawner(30, _enemyContainer,
            new EnemyFactory(levelConfigsList.EnemyTemplate, _player, healthViewFactory, positionViewFactory));

        _enemySpawner = spawner;
        _enemySpawner.Start();
    }

    public void Update(float deltaTime)
    {
        _updatableContainer.Update(deltaTime);
        _enemyContainer.Update(deltaTime);
        _enemySpawner.Update();
    }
}