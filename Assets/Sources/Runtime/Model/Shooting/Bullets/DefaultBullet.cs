using System;
using Model.Characters.CharacterHealth;
using Model.Shooting.Shooter;
using Model.SpatialObject;

namespace Model.Shooting.Bullets
{
    public class DefaultBullet : IBullet
    {
        private float _distance;

        private ITrajectory _trajectory;
        private float _speed;
        private float _damage;
        private float _passedTime;
        private IShooter _shooter;

        public DefaultBullet(Transform transform, ITrajectory trajectory, IShooter shooter, float speed = 0,
            int damage = 0)
        {
            Transform = transform ?? throw new ArgumentNullException();
            Reset(trajectory, speed, damage, shooter);
        }
        
        public bool Collided { get; private set; }
        public Transform Transform { get; }

        public void Reset(ITrajectory trajectory, float speed, int damage, IShooter shooter)
        {
            _trajectory = trajectory ?? throw new ArgumentNullException();
            _shooter = shooter ?? throw new ArgumentNullException(nameof(shooter));

            if (_speed < 0)
                throw new ArgumentOutOfRangeException();

            if (damage < 0)
                throw new ArgumentException();

            _speed = speed;
            _damage = damage;

            _passedTime = 0;
            Collided = false;
            _distance = trajectory.Distance;
            UpdatePosition(0);
        }

        public void UpdateTime(float deltaTime)
        {
            _passedTime += deltaTime;
            UpdatePassedDistance(_passedTime);
        }

        public void Hit(IDamageable damageable)
        {
            bool canHit = _shooter.CanHit(damageable);

            if(canHit == false)
                return;
            
            if (damageable.CanTakeDamage())
                damageable.TakeDamage(_damage);

            Collided = true;
        }

        private void UpdatePassedDistance(float passedTime)
        {
            float normalizedPassedDistance = GetNormalizedPassedDistance(passedTime);
            UpdatePosition(normalizedPassedDistance);

            if (normalizedPassedDistance >= 1)
                Collided = true;
        }

        private float GetNormalizedPassedDistance(float passedTime) =>
            Math.Clamp(passedTime * _speed / _distance, 0, 1);

        private void UpdatePosition(float ratio) => 
            Transform.SetPosition(_trajectory.EvaluateForNormalizedRatio(ratio));
    }
}