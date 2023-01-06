using Model.Characters.CharacterHealth;

namespace Model.Characters.Enemy
{
    public class DefaultAttack : IAttacker
    {
        public bool CanAttack(IDamageable damageable) =>
            damageable.CanTakeDamage();

        public void Attack(IDamageable damageable, float baseDamage)
        {
            if (damageable.CanTakeDamage())
                damageable.TakeDamage(baseDamage);
        }
    }
}