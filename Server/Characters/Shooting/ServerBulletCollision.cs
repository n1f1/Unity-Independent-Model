using System;
using System.Diagnostics;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Server.Simulation;
using Server.Simulation.Physics;

namespace Server.Characters.Shooting
{
    internal class ServerBulletCollision : ICollision
    {
        private readonly IBullet _bullet;

        public ServerBulletCollision(IBullet bullet)
        {
            _bullet = bullet ?? throw new ArgumentNullException(nameof(bullet));
        }

        public void Collide(object collided)
        {
            if (collided is Player player)
                _bullet.Hit(player.Damageable);
        }
    }
}