using System;
using Model.Characters.CharacterHealth;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Networking.Common;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting
{
    internal class RemoteFiredBullet : IBullet
    {
        private const float CatchUpSpeed = 2f;

        private readonly IBullet _bullet;

        private float _realTime;
        private float _aheadServer = NetworkConstants.RTT;

        public RemoteFiredBullet(IBullet bullet)
        {
            _bullet = bullet ?? throw new ArgumentNullException(nameof(bullet));
        }

        public void Hit(IDamageable damageable)
        {
            _bullet.Hit(damageable);
        }

        public bool Collided => _bullet.Collided;
        public Transform Transform => _bullet.Transform;
        public IBullet Bullet => _bullet;

        public void UpdateTime(float deltaTime)
        {
            if (_aheadServer > 0)
            {
                _aheadServer += deltaTime;
                deltaTime *= 1 + _aheadServer / NetworkConstants.RTT * CatchUpSpeed;

                if (_aheadServer < deltaTime)
                    deltaTime = _aheadServer;

                _aheadServer -= deltaTime;
            }

            _bullet.UpdateTime(deltaTime);
        }
    }
}