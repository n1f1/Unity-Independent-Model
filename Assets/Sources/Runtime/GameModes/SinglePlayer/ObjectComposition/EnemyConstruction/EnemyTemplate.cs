using Model.Characters.Enemy;
using Simulation.Pool;
using UnityEngine;
using Utility;

namespace GameModes.SinglePlayer.ObjectComposition.EnemyConstruction
{
    public class EnemyTemplate : MonoBehaviour, IPoolable
    {
        [SerializeField] private MonoBehaviour _enemyViewBehavior;
        [SerializeField] private MonoBehaviour _enemySimulationBehaviour;

        public IEnemyView EnemyView => (IEnemyView) _enemyViewBehavior;
        public IEnemySimulation EnemySimulation => (IEnemySimulation) _enemySimulationBehaviour;

        protected virtual void OnValidate()
        {
            InspectorInterfaceInjection.TrySetObject<IEnemyView>(ref _enemyViewBehavior);
            InspectorInterfaceInjection.TrySetObject<IEnemySimulation>(ref _enemySimulationBehaviour);
        }

        public void Enable() => 
            gameObject.SetActive(true);

        public void Disable() => 
            gameObject.SetActive(false);
    }
}