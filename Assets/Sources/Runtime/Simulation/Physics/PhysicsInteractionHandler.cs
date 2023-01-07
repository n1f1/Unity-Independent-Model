using System;
using Model;
using Model.Physics;
using UnityEngine;

namespace Simulation.Physics
{
    public abstract class PhysicsInteractionHandler<TType> : MonoBehaviour, ISimulation<PhysicsInteraction<TType>>
    {
        protected PhysicsInteraction<TType> PhysicsInteraction;

        public void Initialize(PhysicsInteraction<TType> physicsInteraction)
        {
            PhysicsInteraction = physicsInteraction ?? throw new ArgumentException();
        }
    }
}