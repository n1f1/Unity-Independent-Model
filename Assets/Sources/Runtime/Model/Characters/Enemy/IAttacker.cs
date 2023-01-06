using Model.Characters.CharacterHealth;

namespace Model.Characters.Enemy
{
    public interface IAttacker
    {
        bool CanAttack(IDamageable damageable);
        void Attack(IDamageable damageable, float baseDamage);
    }
}