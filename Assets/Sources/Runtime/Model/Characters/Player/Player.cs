using System;
using Model.Characters.CharacterHealth;
using Model.Shooting;
using Model.SpatialObject;

namespace Model.Characters.Player
{
    public class Player : IUpdatable
    {
        public const int MAXHealth = 100;
        public const float ShootingCooldown = 0.1f;
        public const float CharacterSpeed = 5f;

        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly IDamageable _damageable;
        private readonly CharacterShooter _shooter;

        public Player(Transform transform, IDamageable damageable, CharacterShooter characterShooter)
        {
            _damageable = damageable ?? throw new ArgumentNullException(nameof(damageable));
            _transform = transform ?? throw new ArgumentNullException(nameof(transform));
            _shooter = characterShooter ?? throw new ArgumentNullException(nameof(characterShooter));
            _characterMovement = new CharacterMovement(_transform, CharacterSpeed);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Damageable => _damageable;
        public CharacterShooter CharacterShooter => _shooter;
        public IWeapon Weapon => CharacterShooter.Weapon;
        public IAim Aim => CharacterShooter.Aim;

        public void UpdateTime(float deltaTime)
        {
            _shooter.UpdateTime(deltaTime);
        }
    }
}