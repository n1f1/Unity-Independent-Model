using Model.Shooting.Bullets;
using Simulation.Pool;

namespace Simulation.Shooting.Bullets
{
    public static class BulletFactoryCreation
    {
        public static PooledBulletFactory CreatePooledFactory(BulletTemplate bulletTemplate)
        {
            PooledBulletFactory bulletFactory = new PooledBulletFactory(bulletTemplate,
                new KeyPooledObjectPool<DefaultBullet, SimulationObject>(64));

            bulletFactory.PopulatePool();
            return bulletFactory;
        }
    }
}