using Model.Characters.Shooting.Bullets;
using Simulation;
using Simulation.Pool;

namespace GameModes.SinglePlayer.ObjectComposition.Bullets
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