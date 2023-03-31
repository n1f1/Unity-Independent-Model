using System;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Common;
using SimulationObject;
using Transform = Model.SpatialObject.Transform;

namespace ObjectComposition
{
    public class PooledBulletFactory : IBulletFactory<DefaultBullet>, IBulletDestroyer
    {
        private readonly ISimulationProvider<DefaultBullet> _bulletSimulationProvider;
        private readonly SimulatedSimulationPool<DefaultBullet> _objectPool;

        public PooledBulletFactory(SimulatedSimulationPool<DefaultBullet> objectPool,
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

        public DefaultBullet CreateBullet(ITrajectory trajectory, float speed, int damage)
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
                _objectPool.Return(defaultBullet);
        }

        private DefaultBullet GetFromPool() =>
            _objectPool.GetFree().TObject;

        private void AddNewToObjectPool()
        {
            SimulationObject<DefaultBullet> simulation = _bulletSimulationProvider.CreateSimulationObject();

            DefaultBullet defaultBullet =
                new DefaultBullet(new Transform(simulation.GetView<IPositionView>()), new NullTrajectory());

            _bulletSimulationProvider.InitializeSimulation(simulation, defaultBullet);
            _objectPool.AddNewPair(defaultBullet, simulation);
        }
    }
}