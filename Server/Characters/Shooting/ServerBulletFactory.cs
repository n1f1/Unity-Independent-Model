using System;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.Shooting.Shooter;
using Model.Shooting.Trajectory;
using Model.SpatialObject;
using Server.Characters.ClientPlayer;
using Server.Simulation;
using Server.Simulation.Physics;

namespace Server.Characters.Shooting
{
    internal class ServerBulletFactory : IBulletFactory<IBullet>
    {
        private readonly IPhysicsSimulation _physicsSimulation;

        public ServerBulletFactory(IPhysicsSimulation physicsSimulation)
        {
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
        }

        public IBullet CreateBullet(ITrajectory trajectory, float speed, int damage, IShooter shooter)
        {
            IRigidbody rigidbody = _physicsSimulation.CreateCapsuleRigidbody(0.3f);

            IBullet bullet = new DefaultBullet(new Transform(new UpdateRigidbody(rigidbody)), trajectory,
                shooter, speed, damage);

            _physicsSimulation.RegisterCollidable(rigidbody, bullet);
            _physicsSimulation.AddCollision(rigidbody, new ServerBulletCollision(bullet));

            return bullet;
        }
    }
}