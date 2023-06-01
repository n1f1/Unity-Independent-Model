using System;
using Model.Characters.CharacterHealth;
using Model.Shooting;

namespace Model.Characters.Enemy
{
    public class CooldownAttack : IAttacker
    {
        private readonly IAttacker _attacker;
        private readonly Cooldown _cooldown;

        public CooldownAttack(Cooldown cooldown, IAttacker attacker)
        {
            _attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            _cooldown = cooldown ?? throw new ArgumentNullException(nameof(cooldown));
        }

        public bool CanAttack(IDamageable damageable) =>
            _cooldown.IsReady && _attacker.CanAttack(damageable);

        public void Attack(IDamageable damageable, float baseDamage)
        {
            _attacker.Attack(damageable, baseDamage);
            _cooldown.Reset();
        }
    }
}