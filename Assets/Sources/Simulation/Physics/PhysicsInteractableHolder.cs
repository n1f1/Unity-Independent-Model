using UnityEngine;

namespace Simulation.Physics
{
    internal class PhysicsInteractableHolder<T> : MonoBehaviour
    {
        public T InteractableObject { get; private set; }

        public void Initialize(T tObject)
        {
            InteractableObject = tObject;
        }
    }
}