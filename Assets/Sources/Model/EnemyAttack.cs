namespace Model
{
    public class EnemyAttack
    {
        private readonly IDamageable _target;
        private readonly IAttacker _attacker;
        private float _baseDamage = 10f;

        public EnemyAttack(IDamageable target, IAttacker attacker)
        {
            _target = target;
            _attacker = attacker;
        }

        public void Attack()
        {
            if (_attacker.CanAttack())
                _attacker.Attack(_target, _baseDamage);
        }
    }
}