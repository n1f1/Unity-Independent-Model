using System.Numerics;

namespace Server.Simulation.Physics
{
    internal interface IRigidbody
    {
        void UpdatePosition(Vector3 position);
    }
}