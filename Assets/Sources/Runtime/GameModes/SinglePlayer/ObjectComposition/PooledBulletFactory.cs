using System;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation;
using Simulation.Pool;
using Transform = Model.SpatialObject.Transform;

namespace GameModes.SinglePlayer.ObjectComposition
{
    public class PooledBulletFactory : IBulletFactory<IBullet>
    {
        private readonly ISimulationProvider<DefaultBullet> _bulletSimulationProvider;
        private readonly KeyPooledObjectPool<DefaultBullet, SimulationObject> _objectPool;

        public PooledBulletFactory(KeyPooledObjectPool<DefaultBullet, SimulationObject> objectPool,
            BulletSimulationProvider bulletSimulationProvider)
        {
            _objectPool = objectPool ?? throw new ArgumentException();
            _bulletSimulationProvider = bulletSimulationProvider ?? throw new ArgumentNullException();
        }

        public void PopulatePool()
        {
            for (int i = 0; i < _objectPool.Capacity; i++)
                AddNewToObjectPool();
        }

        public IBullet CreateBullet(ITrajectory trajectory, float speed, int damage)
        {
            if (!_objectPool.CanGet())
                AddNewToObjectPool();

            DefaultBullet defaultBullet = GetFromPool();
            defaultBullet.Reset(trajectory, speed, damage);

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
            SimulationObject simulation = _bulletSimulationProvider.CreateSimulationObject();

            DefaultBullet defaultBullet =
                new DefaultBullet(new Transform(simulation.GetView<IPositionView>()), new NullTrajectory());

            _bulletSimulationProvider.InitializeSimulation(simulation, defaultBullet);
            _objectPool.AddNew(defaultBullet, simulation);
        }
    }
}