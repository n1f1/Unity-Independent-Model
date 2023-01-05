using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Common;
using Simulation.Physics;
using Simulation.Shooting;
using UnityEngine;
using View.Factories;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;

public class PooledBulletFactory : IBulletFactory<DefaultBullet>
{
    private readonly IViewFactory<IPositionView> _viewFactory;
    private readonly GameObject _bulletTemplate;
    private readonly SimulatedObjectPool<DefaultBullet> _objectPool;
    private readonly UpdatableContainer _updatableContainer;

    public PooledBulletFactory(IViewFactory<IPositionView> viewFactory, GameObject bulletTemplate,
        SimulatedObjectPool<DefaultBullet> objectPool,
        UpdatableContainer updatableContainer)
    {
        _updatableContainer = updatableContainer ?? throw new ArgumentException();
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

        SimulatedObjectPool<DefaultBullet>.SimulatedPair simulatedPair = _objectPool.Get();
        DefaultBullet defaultBullet = simulatedPair.TObject;
        defaultBullet.Reset(trajectory, speed, damage);
        _updatableContainer.QueryAdd(defaultBullet);

        return defaultBullet;
    }

    private void AddNewToObjectPool()
    {
        GameObject bullet = Object.Instantiate(_bulletTemplate);
        IPositionView positionView = _viewFactory.Create(bullet);
        CollisionEnter<IDamageable> physicsHandler = bullet.AddComponent<DamageableCollisionEnter>();
        DefaultBullet defaultBullet = new DefaultBullet(new Transform(positionView), null, 0, 0);
        BulletDestroyer bulletDestroyer = bullet.AddComponent<BulletDestroyer>();
        bulletDestroyer.Initialize(defaultBullet,
            new RemoveFromBulletContainer(_updatableContainer, new ReturnToPool<DefaultBullet>(_objectPool)));
        _updatableContainer.QueryAdd(bulletDestroyer);

        physicsHandler.Initialize(new BulletCollisionEnter(defaultBullet));

        _objectPool.Add(defaultBullet, bullet);
    }
}