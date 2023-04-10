﻿using GameModes.SinglePlayer.ObjectComposition;
using Model.Characters.Shooting.Bullets;
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
                    new KeyPooledObjectPool<DefaultBullet, SimulationObject>(64),
                    new BulletSimulationProvider(bulletTemplate, new PositionViewFactory()));

            bulletFactory.PopulatePool();
            return bulletFactory;
        }
    }
}