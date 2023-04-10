using Model;
using Model.Characters.CharacterHealth;
using Simulation.Physics;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition.EnemyConstruction
{
    class EnemySimulation : MonoBehaviour, IEnemySimulation
    {
        private void Awake()
        {
            Damageable = gameObject.AddComponent<DamageablePhysicsInteractableHolder>();
        }

        public ISimulation<IDamageable> Damageable { get; set; }
    }
}