using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    class DistanceAttack : IAttacker
    {
        private readonly IAttacker _attacker;
        private readonly Transform _followTarget;
        private readonly Transform _transform;
        private readonly float _attackRange = 2f;

        public DistanceAttack(Transform followTarget, Transform transform, IAttacker attacker)
        {
            _transform = transform;
            _followTarget = followTarget;
            _attacker = attacker;
        }

        public bool CanAttack()
        {
            return IsDistanceValid() && _attacker.CanAttack();
        }

        private bool IsDistanceValid()
        {
            return Vector3.DistanceSquared(_followTarget.Position, _transform.Position) < _attackRange * _attackRange;
        }

        public void Attack(IDamageable damageable, float baseDamage)
        {
            _attacker.Attack(damageable, baseDamage);
        }
    }
}