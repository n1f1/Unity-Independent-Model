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
        private readonly BulletsContainer _bulletsContainer;

        public Player(IPositionView positionView, IHealthView healthView, ForwardAim forwardAim,
            IBulletDestroyer bulletDestroyer, IBulletFactory<IBullet> bulletFactory)
        {
            _health = new Health(100, healthView ?? throw new ArgumentException(), new Death());
            _transform = new Transform(positionView ?? throw new ArgumentException());

            _cooldown = new Cooldown(0.1f);
            _bulletsContainer = new BulletsContainer(bulletDestroyer);
            _shooter = new CharacterShooter(
                new DefaultGun(forwardAim ?? throw new ArgumentException(),
                    bulletFactory ?? throw new ArgumentException(), _cooldown, _bulletsContainer),
                _transform);

            _characterMovement = new CharacterMovement(_transform, 5f);
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