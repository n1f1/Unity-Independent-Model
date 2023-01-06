using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;

namespace Model.Characters
{
    public class Player : IUpdatable
    {
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly IDamageable _health;
        private readonly CharacterShooter _shooter;
        private readonly Cooldown _cooldown;

        public Player(IPositionView positionView, IHealthView healthView, ForwardAim forwardAim,
            IBulletFactory<IBullet> bulletFactory)
        {
            _health = new Health(100, healthView ?? throw new ArgumentException(), new Death());
            _transform = new Transform(positionView ?? throw new ArgumentException());

            _cooldown = new Cooldown(0.1f);
            _shooter = new CharacterShooter(
                new DefaultGun(forwardAim ?? throw new ArgumentException(),
                    bulletFactory ?? throw new ArgumentException(), _cooldown), _transform);
            
            _characterMovement = new CharacterMovement(_transform, 5f);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Health => _health;
        public CharacterShooter CharacterShooter => _shooter;
        
        public void UpdatePassedTime(float deltaTime)
        {
            _cooldown.ReduceTime(deltaTime);
        }
    }
}