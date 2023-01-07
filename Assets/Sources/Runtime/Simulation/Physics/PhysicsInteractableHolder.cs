using System;
using Model;
using UnityEngine;

namespace Simulation.Physics
{
    internal class PhysicsInteractableHolder<T> : MonoBehaviour, ISimulation<T>
    {
        public T InteractableObject { get; private set; }

        public void Initialize(T tObject)
        {
            InteractableObject = tObject ?? throw new ArgumentException();
        }
    }
}