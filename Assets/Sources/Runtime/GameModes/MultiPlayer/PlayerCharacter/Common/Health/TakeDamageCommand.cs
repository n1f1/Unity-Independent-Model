using System;
using Model.Characters.CharacterHealth;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Health
{
    public class TakeDamageCommand : ICommand
    {
        public TakeDamageCommand(IDamageable damageable, float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            Damageable = damageable ?? throw new ArgumentNullException(nameof(damageable));
            Damage = damage;
        }

        public IDamageable Damageable { get; }
        public float Damage { get; }

        public void Execute()
        {
            Damageable.TakeDamage(Damage);
        }
    }
}