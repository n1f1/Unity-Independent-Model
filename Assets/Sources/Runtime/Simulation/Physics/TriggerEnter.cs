using Model.Physics;

namespace Simulation.Physics
{
    internal class TriggerEnter<TCollisionObject> : PhysicsInteractionHandler<Trigger<TCollisionObject>>
    {
        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (other.gameObject.TryGetComponent(out PhysicsInteractableHolder<TCollisionObject> type))
                PhysicsInteraction.Invoke(new Trigger<TCollisionObject> {Other = type.InteractableObject});
        }
    }
}