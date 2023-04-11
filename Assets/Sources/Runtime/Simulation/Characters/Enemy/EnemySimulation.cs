using Model.Characters.CharacterHealth;
using Simulation.Physics;
using UnityEngine;

namespace Simulation.Characters.Enemy
{
    class EnemySimulation : MonoBehaviour, IEnemySimulation
    {
        public ISimulation<IDamageable> Damageable { get; set; }

        private void Awake()
        {
            Damageable = gameObject.AddComponent<DamageablePhysicsInteractableHolder>();
        }
    }
}