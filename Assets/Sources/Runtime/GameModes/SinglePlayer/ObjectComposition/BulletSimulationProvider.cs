﻿using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting.Bullets;
using Model.Physics;
using Model.SpatialObject;
using Simulation;
using Simulation.Physics;
using Simulation.View.Factories;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameModes.SinglePlayer.ObjectComposition
{
    public class BulletSimulationProvider : ISimulationProvider<DefaultBullet>
    {
        private readonly GameObject _bulletTemplate;
        private readonly IViewFactory<IPositionView> _viewFactory;

        public BulletSimulationProvider(GameObject bulletTemplate, IViewFactory<IPositionView> viewFactory)
        {
            _viewFactory = viewFactory ?? throw new ArgumentNullException();
            _bulletTemplate = bulletTemplate ? bulletTemplate : throw new ArgumentException();
        }

        public SimulationObject CreateSimulationObject()
        {
            GameObject bullet = Object.Instantiate(_bulletTemplate);
            SimulationObject simulation = new SimulationObject(bullet);
            simulation.Add(_viewFactory.Create(bullet));
            TriggerEnter<IDamageable> physicsHandler = bullet.AddComponent<DamageableTriggerEnter>();
            simulation.AddSimulation(physicsHandler);

            return simulation;
        }

        public void InitializeSimulation(SimulationObject simulation, DefaultBullet simulated)
        {
            simulation.GetSimulation<PhysicsInteraction<Trigger<IDamageable>>>()
                .Initialize(new BulletCollisionEnter(simulated));
        }
    }
}