using System;
using System.Collections.Generic;

namespace Model.Shooting.Bullets
{
    public class BulletsContainer
    {
        private readonly LinkedList<IBullet> _bullets = new();
        private readonly IBulletDestroyer _bulletDestroyer;

        public BulletsContainer(IBulletDestroyer bulletDestroyer)
        {
            _bulletDestroyer = bulletDestroyer ?? throw new ArgumentNullException(nameof(bulletDestroyer));
        }

        public void Add(IBullet bullet)
        {
            _bullets.AddLast(bullet);
        }

        public void Update(float deltaTime)
        {
            UpdateBulletsAndProcessCollisions(deltaTime);
        }

        private void UpdateBulletsAndProcessCollisions(float deltaTime)
        {
            for (LinkedListNode<IBullet> node = _bullets.First; node != null; node = node.Next)
            {
                IBullet bullet = node.Value;

                bullet.UpdateTime(deltaTime);

                if (bullet.Collided)
                {
                    _bulletDestroyer.Destroy(bullet);
                    _bullets.Remove(node);
                }
            }
        }
    }
}