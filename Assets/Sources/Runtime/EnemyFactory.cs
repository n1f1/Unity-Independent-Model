using System;
using Model;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using Simulation.Common;
using Simulation.Physics;
using UnityEngine;
using View.Factories;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

public class EnemyFactory : IEnemyFactory
{
    private readonly IViewFactory<IHealthView> _healthViewFactory;
    private readonly IViewFactory<IPositionView> _positionViewFactory;
    private readonly GameObject _enemyTemplate;
    private readonly Player _player;
    private readonly SimulatedSimulationPool<Enemy> _objectPool = new(16);

    public EnemyFactory(GameObject enemyTemplate, Player player, IViewFactory<IHealthView> healthViewFactory,
        IViewFactory<IPositionView> positionViewFactory)
    {
        _enemyTemplate = enemyTemplate ? enemyTemplate : throw new ArgumentNullException();
        _positionViewFactory = positionViewFactory ?? throw new ArgumentNullException();
        _healthViewFactory = healthViewFactory ?? throw new ArgumentNullException();
        _player = player ?? throw new ArgumentNullException();
    }

    public Enemy Create(Vector3 position)
    {
        if (!_objectPool.CanGet())
            AddNewToObjectPool(position);
        
        SimulatedSimulationPool<Enemy>.SimulatedPair simulatedPair = _objectPool.Get();
        Enemy enemy = CreateNewEnemy(position, simulatedPair.Poolable);
        _objectPool.Replace(simulatedPair.TObject, enemy);

        return enemy;
    }

    private void AddNewToObjectPool(Vector3 position)
    {
        SimulationObject<Enemy> simulation = CreateSimulationObject();
        Enemy enemy = CreateNewEnemy(position, simulation);
        _objectPool.AddNew(enemy, simulation);
    }

    private Enemy CreateNewEnemy(Vector3 position, SimulationObject<Enemy> simulation)
    {
        IDeath death = new Death();
        Health health = new Health(100f, simulation.GetView<IHealthView>(), death);
        Transform transform = new Transform(simulation.GetView<IPositionView>(), position);
        Enemy enemy = new Enemy(transform, health, death, _player.Transform, _player.Health);

        simulation.GetSimulation<IDamageable>().Initialize(health);
        return enemy;
    }

    private SimulationObject<Enemy> CreateSimulationObject()
    {
        GameObject enemyObject = Object.Instantiate(_enemyTemplate);
        SimulationObject<Enemy> simulation = new SimulationObject<Enemy>(enemyObject);
        simulation.Add(_positionViewFactory.Create(enemyObject));
        simulation.Add(_healthViewFactory.Create(enemyObject));
        ISimulation<IDamageable> interactableHolder = enemyObject.AddComponent<DamageablePhysicsInteractableHolder>();
        simulation.AddSimulation(interactableHolder);
        
        return simulation;
    }

    public void Destroy(Enemy enemy)
    {
        _objectPool.Return(enemy);
    }
}