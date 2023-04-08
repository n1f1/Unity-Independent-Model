using System;
using Model;
using Model.Physics;
using UnityEngine;

namespace Simulation.Physics
{
    public abstract class PhysicsInteractionHandler<TInteractionType> : MonoBehaviour,
        ISimulation<PhysicsInteraction<TInteractionType>>
    {
        protected PhysicsInteraction<TInteractionType> PhysicsInteraction;

        public void Initialize(PhysicsInteraction<TInteractionType> enemyPlayerPrediction)
        {
            PhysicsInteraction = enemyPlayerPrediction ?? throw new ArgumentException();
        }
    }
}