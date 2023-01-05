using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class Enemy
    {
        private readonly FollowTarget _followTarget;
        private readonly EnemyAttack _enemyAttack;
        private readonly CooldownAttack _cooldownAttack;
        private Health _health;

        public Enemy(Health health, IPositionView positionView, Transform followTarget, IDamageable target)
        {
            _health = health;
            Transform transform = new Transform(positionView);
            transform.SetPosition(5, 5);
            _cooldownAttack = new CooldownAttack(3f, new DistanceAttack(followTarget, transform, new DefaultAttack()));
            _enemyAttack = new EnemyAttack(target, _cooldownAttack);
            _followTarget = new FollowTarget(followTarget, transform, new CharacterMovement(transform, 4f));
        }

        public void Update(float deltaTime)
        {
            _followTarget.Follow(deltaTime);
            _cooldownAttack.AddTime(deltaTime);
            _enemyAttack.Attack();
        }
    }
}