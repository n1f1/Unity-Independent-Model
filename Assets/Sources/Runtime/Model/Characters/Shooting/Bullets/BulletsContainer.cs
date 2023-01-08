using System.Collections.Generic;

namespace Model.Characters.Shooting.Bullets
{
    public class BulletsContainer
    {
        private readonly LinkedList<IBullet> _bullets = new();
        private readonly IBulletDestroyer _bulletDestroyer;

        public BulletsContainer(IBulletDestroyer bulletDestroyer)
        {
            _bulletDestroyer = bulletDestroyer;
        }

        public void Add(IBullet bullet)
        {
            _bullets.AddLast(bullet);
        }

        public void Update(float deltaTime)
        {
            Stack<IBullet> collidedBullets = UpdateBulletsAndProcessCollisions(deltaTime);
            DestroyBullets(collidedBullets);
        }

        private void DestroyBullets(Stack<IBullet> collidedBullets)
        {
            while (collidedBullets.Count > 0)
                _bulletDestroyer.Destroy(collidedBullets.Pop());
        }

        private Stack<IBullet> UpdateBulletsAndProcessCollisions(float deltaTime)
        {
            Stack<IBullet> collidedBullets = new Stack<IBullet>(16);

            for (LinkedListNode<IBullet> node = _bullets.First; node != null; node = node.Next)
            {
                IBullet bullet = node.Value;

                bullet.UpdateTime(deltaTime);

                if (bullet.Collided)
                {
                    collidedBullets.Push(bullet);
                    _bullets.Remove(node);
                }
            }

            return collidedBullets;
        }
    }
}