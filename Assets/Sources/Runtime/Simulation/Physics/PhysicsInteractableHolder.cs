using System;
using Model;
using UnityEngine;

namespace Simulation.Physics
{
    internal class PhysicsInteractableHolder<TSimulated> : MonoBehaviour, ISimulation<TSimulated>
    {
        public TSimulated InteractableObject { get; private set; }

        public void Initialize(TSimulated tObject)
        {
            InteractableObject = tObject ?? throw new ArgumentException();
        }
    }
}