using Model.Characters.Shooting.Bullets;
using ObjectComposition;
using Simulation;
using Simulation.Pool;
using Simulation.View.Factories;
using UnityEngine;

namespace GameModes.SinglePlayer
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