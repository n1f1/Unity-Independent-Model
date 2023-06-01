﻿using System;
using Model.Characters.CharacterHealth;

namespace Model.Characters.Enemy
{
    public class EnemyAttack
    {
        private readonly IDamageable _target;
        private readonly IAttacker _attacker;
        private readonly float _baseDamage = 10f;

        public EnemyAttack(IDamageable target, IAttacker attacker)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
        }

        public void Attack()
        {
            if (_attacker.CanAttack(_target))
                _attacker.Attack(_target, _baseDamage);
        }
    }
}