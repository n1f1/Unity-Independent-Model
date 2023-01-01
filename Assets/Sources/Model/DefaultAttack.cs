namespace Model
{
    public class DefaultAttack : IAttacker
    {
        public bool CanAttack() => 
            true;

        public void Attack(IDamageable damageable, float baseDamage) => 
            damageable.TakeDamage(baseDamage);
    }
}