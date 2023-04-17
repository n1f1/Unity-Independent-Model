using System;
using System.Numerics;
using Model.SpatialObject;
using Server.Simulation;
using Server.Simulation.Physics;

namespace Server.Characters.ClientPlayer
{
    internal class UpdateRigidbody : IPositionView
    {
        private readonly IRigidbody _rigidbody;

        public UpdateRigidbody(IRigidbody rigidbody)
        {
            _rigidbody = rigidbody ?? throw new ArgumentNullException(nameof(rigidbody));
        }

        public void UpdatePosition(Vector3 position)
        {
            _rigidbody.UpdatePosition(position);
        }
    }
}