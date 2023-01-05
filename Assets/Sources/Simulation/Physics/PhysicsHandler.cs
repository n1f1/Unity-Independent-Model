using UnityEngine;

public abstract class PhysicsHandler<TType> : MonoBehaviour
{
    protected PhysicsInteraction<TType> PhysicsInteraction;

    public void Initialize(PhysicsInteraction<TType> physicsInteraction)
    {
        PhysicsInteraction = physicsInteraction;
    }
}