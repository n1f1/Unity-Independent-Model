using System.Collections.Generic;

namespace Model.Characters.Shooting.Bullets
{
    public class BulletsContainer
    {
        private readonly LinkedList<IBullet> _bullets = new();

        public void Add(IBullet bullet) =>
            _bullets.AddLast(bullet);

        public void Remove(IBullet bullet) =>
            _bullets.Remove(bullet);

        public void UpdateBullets(float deltaTime)
        {
            foreach (IBullet bullet in _bullets)
                bullet.AddPassedTime(deltaTime);
        }
    }
}