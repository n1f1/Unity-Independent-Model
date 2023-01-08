using System;
using Model.Characters.CharacterHealth;
using Model.Physics;

namespace Model.Characters.Shooting.Bullets
{
    public class BulletCollisionEnter : PhysicsInteraction<Trigger<IDamageable>>
    {
        private readonly IBullet _bullet;

        public BulletCollisionEnter(IBullet bullet)
        {
            _bullet = bullet ?? throw new ArgumentNullException();
        }

        public void Invoke(Trigger<IDamageable> collision)
        {
            _bullet.Hit(collision.Other);
        }
    }
}