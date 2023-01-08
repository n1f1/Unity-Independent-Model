using System;
using Model;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using Simulation.Physics;
using SimulationObject;
using UnityEngine;
using View.Factories;
using Object = UnityEngine.Object;

namespace ObjectComposition
{
    public class EnemySimulationProvider : ISimulationProvider<Enemy>
    {
        private readonly IViewFactory<IPositionView> _positionViewFactory;
        private readonly IViewFactory<IHealthView> _healthViewFactory;
        private readonly GameObject _enemyTemplate;

        public EnemySimulationProvider(GameObject enemyTemplate, IViewFactory<IHealthView> healthViewFactory,
            IViewFactory<IPositionView> positionViewFactory)
        {
            _enemyTemplate = enemyTemplate ? enemyTemplate : throw new ArgumentNullException();
            _positionViewFactory = positionViewFactory ?? throw new ArgumentNullException();
            _healthViewFactory = healthViewFactory ?? throw new ArgumentNullException();
        }

        public SimulationObject<Enemy> CreateSimulationObject()
        {
            GameObject enemyObject = Object.Instantiate(_enemyTemplate);
            SimulationObject<Enemy> simulation = new SimulationObject<Enemy>(enemyObject);
            simulation.Add(_positionViewFactory.Create(enemyObject));
            simulation.Add(_healthViewFactory.Create(enemyObject));
            
            ISimulation<IDamageable> interactableHolder =
                enemyObject.AddComponent<DamageablePhysicsInteractableHolder>();
            simulation.AddSimulation(interactableHolder);

            return simulation;
        }

        public void InitializeSimulation(SimulationObject<Enemy> simulation, Enemy enemy)
        {
            (simulation ?? throw new ArgumentException()).GetSimulation<IDamageable>()
                .Initialize((enemy ?? throw new ArgumentNullException()).Health);
        }
    }
}