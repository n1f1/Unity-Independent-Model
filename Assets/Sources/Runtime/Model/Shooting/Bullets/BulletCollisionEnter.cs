using System;
using Model.Characters.CharacterHealth;
using Model.Physics;

namespace Model.Shooting.Bullets
{
    public class BulletCollisionEnter : IPhysicsInteraction<Trigger<IDamageable>>
    {
        private readonly IBullet _bullet;

        public BulletCollisionEnter(IBullet bullet)
        {
            _bullet = bullet ?? throw new ArgumentNullException(nameof(bullet));
        }

        public void Invoke(Trigger<IDamageable> collision)
        {
            _bullet.Hit(collision.Other);
        }
    }
}