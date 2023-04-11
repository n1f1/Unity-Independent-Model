using Model;
using Model.Characters.CharacterHealth;
using Model.Physics;
using Simulation;
using Simulation.Physics;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition.Bullets
{
    class BulletSimulation : MonoBehaviour, IBulletSimulation
    {
        public ISimulation<IPhysicsInteraction<Trigger<IDamageable>>> Collidable { get; set; }

        private void Awake()
        {
            Collidable = gameObject.AddComponent<DamageableTriggerEnter>();
        }
    }
}