using System;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting;
using Model.Shooting.Bullets;
using Simulation.Shooting.Bullets;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Shooting
{
    public class BulletDestroyer : IBulletDestroyer
    {
        private readonly PooledBulletFactory _bulletFactory;

        public BulletDestroyer(PooledBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        }

        public void Destroy(IBullet bullet)
        {
            switch (bullet)
            {
                case DefaultBullet defaultBullet:
                    _bulletFactory.Destroy(defaultBullet);
                    break;
                case RemoteFiredBullet remoteFiredBullet:
                    Destroy(remoteFiredBullet.Bullet);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}