using System;
using Model;
using UnityEngine;

namespace Simulation.Physics
{
    internal class PhysicsInteractableHolder<TSimulated> : MonoBehaviour, ISimulation<TSimulated>
    {
        public TSimulated InteractableObject { get; private set; }

        public void Initialize(TSimulated enemyPlayerPrediction)
        {
            InteractableObject = enemyPlayerPrediction ?? throw new ArgumentException();
        }
    }
}