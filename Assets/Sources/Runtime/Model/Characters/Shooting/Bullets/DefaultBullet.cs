using System;
using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace Model.Characters.Shooting.Bullets
{
    public class DefaultBullet : IBullet
    {
        private readonly Transform _transform;
        private float _distance;

        private ITrajectory _trajectory;
        private float _speed;
        private float _damage;
        private float _passedTime;

        public DefaultBullet(Transform transform, ITrajectory trajectory, float speed, int damage)
        {
            _transform = transform ?? throw new ArgumentException();
            _speed = speed;
            _trajectory = trajectory;
            _damage = damage;

            if (_trajectory != null)
                _distance = trajectory.Distance;
        }

        public bool ShouldBeDestroyed { get; private set; }

        public void Reset(ITrajectory trajectory, float speed, int damage)
        {
            _passedTime = 0;
            _trajectory = trajectory ?? throw new ArgumentException();
            _distance = trajectory.Distance;
            _speed = speed;
            _damage = damage;
            ShouldBeDestroyed = false;
            UpdatePosition(0);
        }

        public void UpdatePassedTime(float deltaTime)
        {
            _passedTime += deltaTime;
            float ratio = Math.Clamp(_passedTime * _speed / _distance, 0, 1);
            UpdatePosition(ratio);

            if (ratio >= 1)
                ShouldBeDestroyed = true;
        }

        private void UpdatePosition(float ratio) =>
            _transform.SetPosition(_trajectory.Evaluate(ratio));

        public void Hit(IDamageable damageable)
        {
            if(damageable.CanTakeDamage())
                damageable.TakeDamage(_damage);
            
            ShouldBeDestroyed = true;
        }
    }
}