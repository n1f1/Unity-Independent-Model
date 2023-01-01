namespace Model
{
    public class Enemy
    {
        private readonly FollowTarget _followTarget;
        private readonly EnemyAttack _enemyAttack;
        private readonly CooldownAttack _cooldownAttack;

        public Enemy(IPositionView positionView, Transform followTarget, IDamageable target)
        {
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