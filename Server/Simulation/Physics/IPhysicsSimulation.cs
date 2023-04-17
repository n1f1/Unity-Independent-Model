namespace Server.Simulation.Physics
{
    internal interface IPhysicsSimulation
    {
        void Update(float time);
        IRigidbody CreateCapsuleRigidbody(float radius);
        void RegisterCollidable(IRigidbody rigidbody, object body);
        void AddCollision(IRigidbody rigidbody, ICollision collision);
        void Remove(object body);
    }
}