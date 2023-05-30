using System;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.Shooting;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class Enemy : IUpdatable
    {
        private const float MAXHealth = 100f;
        private const int Cooldown = 1;
        private const float Speed = 4f;

        private readonly IDeath _death;
        private readonly EnemyAttack _enemyAttack;
        private readonly FollowTarget _followTarget;
        private readonly Cooldown _cooldown;

        public Enemy(Vector3 position, Player.Player player, IHealthView healthView, IPositionView positionView)
        {
            if (player == null)
                throw new ArgumentNullException();

            Transform transform = new Transform(positionView, position);
            
            _death = new Death(new NullDeathView());
            Health = new Health(MAXHealth, MAXHealth, healthView, _death);
            _cooldown = new Cooldown(Cooldown);

            _enemyAttack = new EnemyAttack(player.Damageable, new CooldownAttack(_cooldown,
                new DistanceAttack(player.Transform, transform, new DefaultAttack())));

            _followTarget = new FollowTarget(player.Transform, transform, new CharacterMovement(transform, Speed));
        }
        
        public Health Health { get; }
        public bool Dead => _death.Dead;

        public void UpdateTime(float deltaTime)
        {
            _followTarget.Follow(deltaTime);
            _cooldown.ReduceTime(deltaTime);
            _enemyAttack.Attack();
        }
    }
}