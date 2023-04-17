using System.Numerics;

namespace Server.Simulation.Physics
{
    internal class RigidBody : IRigidbody
    {
        public RigidBody(float radius)
        {
            Radius = radius;
        }

        public Vector3 Position { get; private set; }
        public float Radius { get; }

        public void UpdatePosition(Vector3 position)
        {
            Position = position;
        }
    }
}