using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.Physics;
using Model.SpatialObject;
using Simulation.Common;
using Simulation.Physics;
using UnityEngine;
using View.Factories;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;

public class PooledBulletFactory : IBulletFactory<DefaultBullet>, IBulletDestroyer
{
    private readonly IViewFactory<IPositionView> _viewFactory;
    private readonly GameObject _bulletTemplate;
    private readonly SimulatedSimulationPool<DefaultBullet> _objectPool;

    public PooledBulletFactory(IViewFactory<IPositionView> viewFactory, GameObject bulletTemplate,
        SimulatedSimulationPool<DefaultBullet> objectPool)
    {
        _viewFactory = viewFactory ?? throw new ArgumentException();
        _objectPool = objectPool ?? throw new ArgumentException();
        _bulletTemplate = bulletTemplate ? bulletTemplate : throw new ArgumentException();
    }

    public void PopulatePool()
    {
        for (int i = 0; i < _objectPool.Capacity; i++)
            AddNewToObjectPool();
    }

    public DefaultBullet CreateBullet(ITrajectory trajectory, float speed, int damage)
    {
        if (!_objectPool.CanGet())
            AddNewToObjectPool();

        SimulatedSimulationPool<DefaultBullet>.SimulatedPair simulatedPair = _objectPool.Get();
        DefaultBullet defaultBullet = simulatedPair.TObject;
        defaultBullet.Reset(trajectory, speed, damage);

        return defaultBullet;
    }

    private void AddNewToObjectPool()
    {
        SimulationObject<DefaultBullet> simulation = CreateSimulationObject();

        DefaultBullet defaultBullet = new DefaultBullet(new Transform(simulation.GetView<IPositionView>()), null, 0, 0);

        simulation.GetSimulation<PhysicsInteraction<Collision<IDamageable>>>()
            .Initialize(new BulletCollisionEnter(defaultBullet));

        _objectPool.AddNew(defaultBullet, simulation);
    }

    private SimulationObject<DefaultBullet> CreateSimulationObject()
    {
        GameObject bullet = Object.Instantiate(_bulletTemplate);
        SimulationObject<DefaultBullet> simulation = new SimulationObject<DefaultBullet>(bullet);
        simulation.Add(_viewFactory.Create(bullet));
        CollisionEnter<IDamageable> physicsHandler = bullet.AddComponent<DamageableCollisionEnter>();
        simulation.AddSimulation(physicsHandler);

        return simulation;
    }

    public void Destroy(IBullet bullet)
    {
        if(bullet is DefaultBullet defaultBullet)
            _objectPool.Return(defaultBullet);
    }
}