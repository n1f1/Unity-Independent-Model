﻿using System;
using Model.Physics;
using UnityEngine;

namespace Simulation.Physics
{
    public abstract class PhysicsInteractionHandler<TInteractionType> : MonoBehaviour,
        ISimulation<IPhysicsInteraction<TInteractionType>>
    {
        protected IPhysicsInteraction<TInteractionType> PhysicsInteraction;

        public ISimulation<IPhysicsInteraction<TInteractionType>> Initialize(
            IPhysicsInteraction<TInteractionType> physicsInteraction)
        {
            PhysicsInteraction = physicsInteraction ?? throw new ArgumentException();

            return this;
        }

        public void UpdateTime(float deltaTime)
        {
        }
    }
}