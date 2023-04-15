using System;
using System.Collections.Generic;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Model.SpatialObject;

namespace Server
{
    internal class PhysicBullet : IBullet
    {
        private readonly IBullet _bullet;

        public PhysicBullet(IBullet bullet)
        {
            _bullet = bullet ?? throw new ArgumentNullException(nameof(bullet));
        }

        public void UpdateTime(float deltaTime)
        {
            _bullet.UpdateTime(deltaTime);
        }

        public void Hit(IDamageable damageable)
        {
            _bullet.Hit(damageable);
        }

        public bool Collided => _bullet.Collided;
        public Transform Transform => _bullet.Transform;

        public void ProcessCollisions(IEnumerable<Player> players)
        {
            if(Collided)
                return;
            
            foreach (Player player in players)
            {
                Vector3 playerPosition = player.Transform.Position;
                playerPosition.Y = 0;
                Vector3 bullet = Transform.Position;
                bullet.Y = 0;
                
                if (Vector3.DistanceSquared(playerPosition, bullet) < 1.21f) 
                    Hit(player.Damageable);
            }
        }
    }
}