using Model;
using UnityEngine;
using View;
using Object = UnityEngine.Object;
using Transform = Model.Transform;

public class PooledBulletFactory : IBulletFactory<DefaultBullet>
{
    private readonly IViewFactory<IPositionView> _viewFactory;
    private readonly GameObject _bulletTemplate;
    private readonly BulletsContainer _bulletsContainer;
    private readonly SimulatedObjectPool<DefaultBullet> _objectPool;
    private UpdatablesContainer _updatablesContainer;

    public PooledBulletFactory(IViewFactory<IPositionView> viewFactory, GameObject bulletTemplate,
        BulletsContainer bulletsContainer, SimulatedObjectPool<DefaultBullet> objectPool,
        UpdatablesContainer updatablesContainer)
    {
        _updatablesContainer = updatablesContainer;
        _bulletsContainer = bulletsContainer;
        _bulletTemplate = bulletTemplate;
        _viewFactory = viewFactory;
        _objectPool = objectPool;
    }

    public void PopulatePool()
    {
        for (int i = 0; i < _objectPool.Capacity; i++)
            AddNewToObjectPool();
    }

    public DefaultBullet CreateBullet(ITrajectory trajectory, float speed, int damage)
    {
        DefaultBullet defaultBullet;

        if (_objectPool.CanGet())
        {
            SimulatedObjectPool<DefaultBullet>.SimulablePair simulablePair = _objectPool.Get();
            defaultBullet = simulablePair.TObject;
            defaultBullet.Reset(trajectory, speed, damage);
        }
        else
        {
            defaultBullet = CreateNotPoolable(trajectory, speed, damage);
        }

        _bulletsContainer.Add(defaultBullet);

        return defaultBullet;
    }

    private DefaultBullet CreateNotPoolable(ITrajectory trajectory, float speed, int damage)
    {
        GameObject bullet = Object.Instantiate(_bulletTemplate);
        IPositionView positionView = _viewFactory.Create(bullet);
        CollisionEnter<IDamageable> physicsHandler = bullet.AddComponent<DamageableCollisionEnter>();
        DefaultBullet defaultBullet = new DefaultBullet(new Transform(positionView), trajectory, speed, damage);
        BulletDestroyer bulletDestroyer = bullet.AddComponent<BulletDestroyer>();
        bulletDestroyer.Initialize(defaultBullet, new RemoveFromBulletContainer(_bulletsContainer, new NullDestroy()));
        _updatablesContainer.Add(bulletDestroyer);
        physicsHandler.Initialize(new BulletCollisionEnter(defaultBullet));

        return defaultBullet;
    }

    private void AddNewToObjectPool()
    {
        GameObject bullet = Object.Instantiate(_bulletTemplate);
        IPositionView positionView = _viewFactory.Create(bullet);
        CollisionEnter<IDamageable> physicsHandler = bullet.AddComponent<DamageableCollisionEnter>();
        DefaultBullet defaultBullet = new DefaultBullet(new Transform(positionView), null, 0, 0);
        BulletDestroyer bulletDestroyer = bullet.AddComponent<BulletDestroyer>();
        bulletDestroyer.Initialize(defaultBullet, new RemoveFromBulletContainer(_bulletsContainer,
            new ReturnToPool<DefaultBullet>(_objectPool)));
        _updatablesContainer.Add(bulletDestroyer);

        physicsHandler.Initialize(new BulletCollisionEnter(defaultBullet));

        _objectPool.Add(defaultBullet, bullet);
    }
}