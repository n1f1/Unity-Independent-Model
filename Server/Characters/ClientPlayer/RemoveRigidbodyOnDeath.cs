using System;
using Model.Characters.CharacterHealth;
using Server.Simulation.Physics;

namespace Server.Characters.ClientPlayer
{
    internal class RemoveRigidbodyOnDeath : IDeathView
    {
        private readonly IPhysicsSimulation _physicsSimulation;
        private readonly IRigidbody _rigidbody;

        public RemoveRigidbodyOnDeath(IRigidbody rigidbody, IPhysicsSimulation physicsSimulation)
        {
            _rigidbody = rigidbody ?? throw new ArgumentNullException(nameof(rigidbody));
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
        }

        public void Die()
        {
            _physicsSimulation.Remove(_rigidbody);
        }
    }
}