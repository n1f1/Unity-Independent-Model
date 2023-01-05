using Model.Physics;

internal class CollisionEnter<TCollisionObject> : PhysicsHandler<Collision<TCollisionObject>>
{
    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other.gameObject.TryGetComponent(out Collidable<TCollisionObject> type))
            PhysicsInteraction.Invoke(new Collision<TCollisionObject> {CollisionObject = type.CollidableObject});
    }
}