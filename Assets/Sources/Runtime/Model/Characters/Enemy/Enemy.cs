using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class Enemy : IUpdatable
    {
        private readonly FollowTarget _followTarget;
        private readonly EnemyAttack _enemyAttack;
        private readonly Cooldown _cooldown;
        private Health _health;
        private IDeath _death;

        public Enemy(Transform enemyTransform, Health health, IDeath death, Transform followTarget, IDamageable target)
        {
            _death = death;
            _health = health ?? throw new ArgumentException();
            Transform transform = enemyTransform;

            _cooldown = new Cooldown(1f);
            CooldownAttack cooldownAttack = new CooldownAttack(_cooldown,
                new DistanceAttack(followTarget, transform, new DefaultAttack()));

            _enemyAttack = new EnemyAttack(target, cooldownAttack);
            _followTarget = new FollowTarget(followTarget, transform, new CharacterMovement(transform, 4f));
        }

        public bool Dead => _death.Dead;

        public void UpdatePassedTime(float deltaTime)
        {
            _followTarget.Follow(deltaTime);
            _cooldown.ReduceTime(deltaTime);
            _enemyAttack.Attack();   
        }
    }
}