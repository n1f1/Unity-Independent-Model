using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class Enemy : IUpdatable
    {
        private readonly IDeath _death;
        private readonly Health _health;
        private readonly EnemyAttack _enemyAttack;
        private readonly FollowTarget _followTarget;
        private readonly Cooldown _cooldown;

        public Enemy(Transform transform, IDeath death, Health health, Player player)
        {
            if (transform == null)
                throw new ArgumentNullException();
            
            if (player == null)
                throw new ArgumentNullException();
            
            _death = death ?? throw new ArgumentNullException();
            _health = health ?? throw new ArgumentNullException();
            _cooldown = new Cooldown(1);

            _enemyAttack = new EnemyAttack(player.Health, new CooldownAttack(_cooldown,
                new DistanceAttack(player.Transform, transform, new DefaultAttack())));

            _followTarget = new FollowTarget(player.Transform, transform, new CharacterMovement(transform, 4f));
        }

        public Health Health => _health;
        public bool Dead => _death.Dead;

        public void UpdateTime(float deltaTime)
        {
            _followTarget.Follow(deltaTime);
            _cooldown.ReduceTime(deltaTime);
            _enemyAttack.Attack();
        }
    }
}