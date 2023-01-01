namespace Model
{
    public interface IAttacker
    {
        bool CanAttack();
        void Attack(IDamageable damageable, float baseDamage);
    }
}