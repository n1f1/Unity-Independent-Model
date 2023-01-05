using System;
using Model.Characters.CharacterHealth;

namespace Model.Characters.Enemy
{
    public class CooldownAttack : IAttacker
    {
        private readonly IAttacker _attacker;
        private readonly float _cooldown;
        private float _time;

        public CooldownAttack(float cooldown, IAttacker attacker)
        {
            _attacker = attacker?? throw new ArgumentException();
            _cooldown = cooldown;
        }

        public bool CanAttack() => 
            _time >= _cooldown && _attacker.CanAttack();

        public void Attack(IDamageable damageable, float baseDamage)
        {
            _attacker.Attack(damageable, baseDamage);
            _time = 0;
        }

        public void AddTime(float deltaTime)
        {
            _time += deltaTime;
        }
    }
}