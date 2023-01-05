using Model.Physics;
using UnityEngine;

namespace Simulation.Physics
{
    public abstract class PhysicsInteractionHandler<TType> : MonoBehaviour
    {
        protected PhysicsInteraction<TType> PhysicsInteraction;

        public void Initialize(PhysicsInteraction<TType> physicsInteraction)
        {
            PhysicsInteraction = physicsInteraction;
        }
    }
}