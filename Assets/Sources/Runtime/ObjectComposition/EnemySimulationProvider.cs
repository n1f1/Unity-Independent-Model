using Model;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using Simulation.Physics;
using SimulationObject;
using UnityEngine;
using View.Factories;

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
            _positionViewFactory = positionViewFactory;
            _healthViewFactory = healthViewFactory;
            _enemyTemplate = enemyTemplate;
        }

        public SimulationObject<Enemy> CreateSimulationObject()
        {
            GameObject enemyObject = Object.Instantiate(_enemyTemplate);
            SimulationObject<Enemy> simulation = new SimulationObject<Enemy>(enemyObject);
            simulation.Add(_positionViewFactory.Create(enemyObject));
            simulation.Add(_healthViewFactory.Create(enemyObject));
            ISimulation<IDamageable> interactableHolder = enemyObject.AddComponent<DamageablePhysicsInteractableHolder>();
            simulation.AddSimulation(interactableHolder);

            return simulation;
        }

        public void InitializeSimulation(SimulationObject<Enemy> simulation, Enemy enemy) => 
            simulation.GetSimulation<IDamageable>().Initialize(enemy.Health);
    }
}