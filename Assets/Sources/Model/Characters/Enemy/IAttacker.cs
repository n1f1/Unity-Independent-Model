using Model.Characters.CharacterHealth;

namespace Model.Characters.Enemy
{
    public interface IAttacker
    {
        bool CanAttack();
        void Attack(IDamageable damageable, float baseDamage);
    }
}