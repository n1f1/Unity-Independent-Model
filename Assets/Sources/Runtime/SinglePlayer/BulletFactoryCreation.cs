using Model.Characters.Shooting.Bullets;
using ObjectComposition;
using Simulation.Pool;
using SimulationObject;
using UnityEngine;
using View.Factories;

namespace SinglePlayer
{
    public class BulletFactoryCreation
    {
        public static PooledBulletFactory CreatePooledBulletFactory(GameObject bulletTemplate)
        {
            PooledBulletFactory bulletFactory =
                new PooledBulletFactory(
                    new KeyPooledObjectPool<DefaultBullet, SimulationObject<DefaultBullet>>(64),
                    new BulletSimulationProvider(bulletTemplate, new PositionViewFactory()));

            bulletFactory.PopulatePool();
            return bulletFactory;
        }
    }
}