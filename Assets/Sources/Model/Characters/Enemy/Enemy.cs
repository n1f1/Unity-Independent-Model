using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class Enemy
    {
        private readonly FollowTarget _followTarget;
        private readonly EnemyAttack _enemyAttack;
        private readonly CooldownAttack _cooldownAttack;
        private readonly Cooldown _cooldown;
        private Health _health;

        public Enemy(Health health, IPositionView positionView, Transform followTarget, IDamageable target)
        {
            _health = health ?? throw new ArgumentException();
            Transform transform = new Transform(positionView);
            transform.SetPosition(5, 5);
            _cooldown = new Cooldown(1f);
            _cooldownAttack =
                new CooldownAttack(_cooldown, new DistanceAttack(followTarget, transform, new DefaultAttack()));
            
            _enemyAttack = new EnemyAttack(target, _cooldownAttack);
            _followTarget = new FollowTarget(followTarget, transform, new CharacterMovement(transform, 4f));
        }

        public void Update(float deltaTime)
        {
            _followTarget.Follow(deltaTime);
            _cooldown.ReduceTime(deltaTime);
            _enemyAttack.Attack();
        }
    }
}