﻿using System;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    class DistanceAttack : IAttacker
    {
        private readonly IAttacker _attacker;
        private readonly Transform _followTarget;
        private readonly Transform _transform;
        private readonly float _attackRange = 2f;

        public DistanceAttack(Transform followTarget, Transform transform, IAttacker attacker)
        {
            _transform = transform ?? throw new ArgumentException();
            _followTarget = followTarget ?? throw new ArgumentException();
            _attacker = attacker ?? throw new ArgumentException();
        }

        public bool CanAttack() => 
            IsDistanceValid() && _attacker.CanAttack();

        private bool IsDistanceValid() => 
            Vector3.DistanceSquared(_followTarget.Position, _transform.Position) < _attackRange * _attackRange;

        public void Attack(IDamageable damageable, float baseDamage)
        {
            _attacker.Attack(damageable, baseDamage);
        }
    }
}