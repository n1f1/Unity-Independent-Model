using System.Collections.Generic;
using Model.Characters.CharacterHealth;

namespace Model.Shooting
{
    public class DamageableShooter : IShooter
    {
        private readonly HashSet<IDamageable> _excludeDamageables = new();

        public bool CanHit(IDamageable damageable) => 
            _excludeDamageables.Contains(damageable) == false;

        public void Exclude(IDamageable playerDamageable) => 
            _excludeDamageables.Add(playerDamageable);
    }
}