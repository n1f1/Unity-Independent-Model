using Model.Physics;

namespace Simulation.Physics
{
    internal class CollisionEnter<TCollisionObject> : PhysicsInteractionHandler<Collision<TCollisionObject>>
    {
        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (other.gameObject.TryGetComponent(out PhysicsInteractableHolder<TCollisionObject> type))
                PhysicsInteraction.Invoke(new Collision<TCollisionObject> {CollisionObject = type.InteractableObject});
        }
    }
}