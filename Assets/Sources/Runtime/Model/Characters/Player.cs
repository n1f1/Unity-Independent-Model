using System;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;

namespace Model.Characters
{
    public class Player : IUpdatable
    {
        private const int MAXHealth = 100;
        private const float ShootingCooldown = 0.1f;
        public const float CharacterSpeed = 5f;

        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly IDamageable _health;
        private readonly CharacterShooter _shooter;
        private readonly Cooldown _cooldown;
        private readonly BulletsContainer _bulletsContainer;

        public Player(IPositionView positionView, IHealthView healthView, ForwardAim forwardAim,
            IBulletDestroyer bulletDestroyer, IBulletFactory<IBullet> bulletFactory, Vector3 position)
        {
            _health = new Health(MAXHealth, healthView ?? throw new ArgumentException(), new Death());
            _transform = new Transform(positionView ?? throw new ArgumentException(), position);

            _cooldown = new Cooldown(ShootingCooldown);
            _bulletsContainer = new BulletsContainer(bulletDestroyer);
            _shooter = new CharacterShooter(
                new DefaultGun(forwardAim ?? throw new ArgumentException(),
                    bulletFactory ?? throw new ArgumentException(), _cooldown, _bulletsContainer),
                _transform);

            _characterMovement = new CharacterMovement(_transform, CharacterSpeed);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Health => _health;
        public CharacterShooter CharacterShooter => _shooter;

        public void UpdateTime(float deltaTime)
        {
            _cooldown.ReduceTime(deltaTime);
            _bulletsContainer.Update(deltaTime);
        }
    }
}