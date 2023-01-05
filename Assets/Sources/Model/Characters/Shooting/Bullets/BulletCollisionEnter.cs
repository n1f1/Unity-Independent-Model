using System;
using Model.Characters.CharacterHealth;
using Model.Physics;

namespace Model.Characters.Shooting.Bullets
{
    public class BulletCollisionEnter : PhysicsInteraction<Collision<IDamageable>>
    {
        private readonly IBullet _bullet;

        public BulletCollisionEnter(IBullet bullet)
        {
            _bullet = bullet ?? throw new ArgumentException();
        }

        public void Invoke(Collision<IDamageable> collision)
        {
            _bullet.Hit(collision.CollisionObject);
        }
    }
}