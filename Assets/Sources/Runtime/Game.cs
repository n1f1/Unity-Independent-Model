using Model.Characters;
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
    private EnemyContainer _enemyContainer;
    private EnemySpawner _enemySpawner;
    private Enemy _enemy;
    private Player _player;
    private LevelConfig _levelConfig;
    private IPositionView _cameraView;
    private IViewFactory<IPositionView> _positionViewFactory;
    private IViewFactory<IHealthView> _healthViewFactory;
    public PlayerFactory PlayerFactory { get; private set; }

    public void Start()
    {
        _levelConfig = Resources.Load<LevelConfig>("LevelConfigsList");
        _cameraView = Camera.main.GetComponentInParent<PositionView>();

        _positionViewFactory = new PositionViewFactory();
        _healthViewFactory = new HealthViewFactory();

        _enemyContainer = new EnemyContainer();

        EnemySimulationProvider enemySimulationProvider =
            new EnemySimulationProvider(_levelConfig.EnemyTemplate, _healthViewFactory, _positionViewFactory);

        /*_enemySpawner = new EnemySpawner(3, _enemyContainer, new EnemyFactory(_player, enemySimulationProvider),
            _player.Transform);

        _enemySpawner.Start();*/
    }

    public void CreatePlayerSimulation(ObjectSender objectSender)
    {
        PlayerFactory = new PlayerFactory(_levelConfig, _positionViewFactory, _healthViewFactory, _cameraView,
            objectSender);
    }

    public void Update(float deltaTime)
    {
        _enemyContainer.UpdateTime(deltaTime);
        //_enemySpawner.Update();
        _player?.UpdateTime(deltaTime);
        PlayerFactory?.Update(deltaTime);
    }

    public void Add(Player player)
    {
        _player = player;
    }
}