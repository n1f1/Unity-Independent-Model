﻿using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;

namespace Model.Characters
{
    public class Player
    {
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly IDamageable _health;
        private readonly CharacterShooter _shooter;

        public Player(IPositionView positionView, IHealthView healthView, ForwardAim forwardAim,
            IBulletFactory<IBullet> bulletFactory)
        {
            _health = new Health(healthView ?? throw new ArgumentException());
            _transform = new Transform(positionView ?? throw new ArgumentException());
            
            _shooter = new CharacterShooter(
                new DefaultGun(forwardAim ?? throw new ArgumentException(),
                    bulletFactory ?? throw new ArgumentException()), _transform);
            
            _characterMovement = new CharacterMovement(_transform, 5f);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Health => _health;
        public CharacterShooter CharacterShooter => _shooter;
    }
}