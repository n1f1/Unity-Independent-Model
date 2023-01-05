using Model.Physics;

namespace Model
{
    public class BulletCollisionEnter : PhysicsInteraction<Collision<IDamageable>>
    {
        private readonly IBullet _bullet;

        public BulletCollisionEnter(IBullet bullet)
        {
            _bullet = bullet;
        }

        public void Invoke(Collision<IDamageable> collision)
        {
            _bullet.Hit(collision.CollisionObject);
        }
    }
}