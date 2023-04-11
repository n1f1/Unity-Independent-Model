﻿using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Networking;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    internal class RemoteFiredBullet : IBullet
    {
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

        public void UpdateTime(float deltaTime)
        {
            if (_aheadServer > 0)
            {
                _aheadServer += deltaTime;
                deltaTime *= (1 + _aheadServer / NetworkConstants.RTT);

                if (_aheadServer < deltaTime)
                    deltaTime = _aheadServer;

                _aheadServer -= deltaTime;
            }

            _bullet.UpdateTime(deltaTime);
        }
    }
}