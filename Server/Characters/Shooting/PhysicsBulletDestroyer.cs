using System;
using Model.Shooting.Bullets;
using Server.Simulation;
using Server.Simulation.Physics;

namespace Server.Characters.Shooting
{
    internal class PhysicsBulletDestroyer : IBulletDestroyer
    {
        private readonly IPhysicsSimulation _physicsSimulation;

        public PhysicsBulletDestroyer(IPhysicsSimulation physicsSimulation)
        {
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
        }

        public void Destroy(IBullet bullet)
        {
            _physicsSimulation.Remove(bullet);
        }
    }
}