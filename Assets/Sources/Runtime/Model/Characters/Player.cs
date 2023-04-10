using System;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;

namespace Model.Characters
{
    public class Player : IUpdatable
    {
        public const int MAXHealth = 100;
        public const float ShootingCooldown = 0.1f;
        public const float CharacterSpeed = 5f;

        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly IDamageable _health;
        private readonly CharacterShooter _shooter;
        private readonly Cooldown _cooldown;
        private readonly IWeapon _weapon;

        public Player(IHealthView healthView, ForwardAim forwardAim,
            IDeathView deathView, IWeapon weapon, Cooldown cooldown, Transform transform)
        {
            _health = new Health(MAXHealth, healthView ?? throw new ArgumentException(), new Death(deathView));
            _transform = transform ?? throw new ArgumentNullException(nameof(transform));

            _cooldown = cooldown ?? throw new ArgumentNullException(nameof(cooldown));
            _weapon = weapon;
            _shooter = new CharacterShooter(forwardAim ?? throw new ArgumentException(), _weapon, _transform);
            Aim = forwardAim;
            _characterMovement = new CharacterMovement(_transform, CharacterSpeed);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Health => _health;
        public CharacterShooter CharacterShooter => _shooter;
        public IWeapon Weapon => _weapon;
        public IAim Aim { get; }

        public void UpdateTime(float deltaTime)
        {
            _cooldown.ReduceTime(deltaTime);
        }
    }
}