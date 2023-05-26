using System;
using Model.Characters.CharacterHealth;
using Model.Shooting;
using Model.Shooting.Shooter;
using Model.SpatialObject;

namespace Model.Characters.Player
{
    public class Player : IUpdatable
    {
        public const int MAXHealth = 100;
        public const float ShootingCooldown = 0.1f;
        public const float CharacterSpeed = 5f;

        private readonly CharacterShooter _characterShooter;

        public Player(Transform transform, Health health, IDamageable damageable, CharacterShooter characterShooter,
            DamageableShooter shooter)
        {
            Health = health ?? throw new ArgumentNullException(nameof(health));
            Shooter = shooter ?? throw new ArgumentNullException(nameof(shooter));
            Damageable = damageable ?? throw new ArgumentNullException(nameof(damageable));
            Transform = transform ?? throw new ArgumentNullException(nameof(transform));
            _characterShooter = characterShooter ?? throw new ArgumentNullException(nameof(characterShooter));
            CharacterMovement = new CharacterMovement(transform, CharacterSpeed);
        }
        
        public DamageableShooter Shooter { get; }
        public Health Health { get; }
        public CharacterMovement CharacterMovement { get; }
        public IDamageable Damageable { get; }
        public Transform Transform { get; }

        public CharacterShooter CharacterCharacterShooter => _characterShooter;

        public void UpdateTime(float deltaTime)
        {
            _characterShooter.UpdateTime(deltaTime);
        }
    }
}