using System;
using UnityEngine;

namespace Simulation.Physics
{
    internal class PhysicsInteractableHolder<TSimulated> : MonoBehaviour, ISimulation<TSimulated>
    {
        public TSimulated InteractableObject { get; private set; }

        public ISimulation<TSimulated> Initialize(TSimulated enemyPlayerPrediction)
        {
            InteractableObject = enemyPlayerPrediction ?? throw new ArgumentException();
            
            return this;
        }

        public void UpdateTime(float deltaTime)
        {
        }
    }
}