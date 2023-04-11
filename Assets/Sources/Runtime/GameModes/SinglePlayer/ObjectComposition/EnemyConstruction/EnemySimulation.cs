using Model;
using Model.Characters.CharacterHealth;
using Simulation;
using Simulation.Physics;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition.EnemyConstruction
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