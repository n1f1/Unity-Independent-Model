using System;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.Shooting.Shooter;
using Simulation.Pool;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;

namespace Simulation.Shooting.Bullets
{
    public class PooledBulletFactory : IBulletFactory<IBullet>
    {
        private readonly KeyPooledObjectPool<DefaultBullet, SimulationObject> _objectPool;
        private readonly BulletTemplate _bulletTemplate;

        public PooledBulletFactory(BulletTemplate bulletTemplate,
            KeyPooledObjectPool<DefaultBullet, SimulationObject> objectPool)
        {
            _bulletTemplate = bulletTemplate;
            _objectPool = objectPool ?? throw new ArgumentException();
        }

        public void PopulatePool()
        {
            for (int i = 0; i < _objectPool.Capacity; i++)
                AddNewToObjectPool();
        }

        public IBullet CreateBullet(ITrajectory trajectory, float speed, int damage, IShooter shooter)
        {
            if (!_objectPool.CanGet())
                AddNewToObjectPool();

            DefaultBullet defaultBullet = GetFromPool();
            defaultBullet.Reset(trajectory, speed, damage, shooter);

            return defaultBullet;
        }

        public void Destroy(IBullet bullet)
        {
            if (bullet is DefaultBullet defaultBullet)
                _objectPool.ReturnInactive(defaultBullet);
        }

        private DefaultBullet GetFromPool() =>
            _objectPool.GetFreeByKey();

        private void AddNewToObjectPool()
        {
            BulletTemplate bulletTemplate = Object.Instantiate(_bulletTemplate);
            SimulationObject simulation = new SimulationObject(bulletTemplate.gameObject);

            DefaultBullet defaultBullet =
                new DefaultBullet(new Transform(bulletTemplate.BulletView.PositionView), new NullTrajectory(),
                    new DamageableShooter());

            simulation.AddUpdatableSimulation(
                bulletTemplate.BulletSimulation.Collidable.Initialize(new BulletCollisionEnter(defaultBullet)));

            _objectPool.AddNew(defaultBullet, simulation);
        }
    }
}