using Model.Characters.CharacterHealth;
using Model.Characters.Shooting.Bullets;
using Model.Physics;
using Model.SpatialObject;
using Simulation.Physics;
using SimulationObject;
using UnityEngine;
using View.Factories;

namespace ObjectComposition
{
    public class BulletSimulationProvider : ISimulationProvider<DefaultBullet>
    {
        private readonly GameObject _bulletTemplate;
        private readonly IViewFactory<IPositionView> _viewFactory;

        public BulletSimulationProvider(GameObject bulletTemplate, IViewFactory<IPositionView> viewFactory)
        {
            _viewFactory = viewFactory;
            _bulletTemplate = bulletTemplate;
        }

        public SimulationObject<DefaultBullet> CreateSimulationObject()
        {
            GameObject bullet = Object.Instantiate(_bulletTemplate);
            SimulationObject<DefaultBullet> simulation = new SimulationObject<DefaultBullet>(bullet);
            simulation.Add(_viewFactory.Create(bullet));
            TriggerEnter<IDamageable> physicsHandler = bullet.AddComponent<DamageableTriggerEnter>();
            simulation.AddSimulation(physicsHandler);

            return simulation;
        }

        public void InitializeSimulation(SimulationObject<DefaultBullet> simulation, DefaultBullet simulated)
        {
            simulation.GetSimulation<PhysicsInteraction<Trigger<IDamageable>>>()
                .Initialize(new BulletCollisionEnter(simulated));   
        }
    }
}