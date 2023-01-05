using System;

namespace Model
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
            _speed = speed;
            _trajectory = trajectory;
            _transform = transform;
            _damage = damage;
            
            if(_trajectory != null)
                _distance = trajectory.Distance;
        }

        public bool ShouldBeDestroyed { get; private set; }

        public void Reset(ITrajectory trajectory, float speed, int damage)
        {
            _passedTime = 0;
            _trajectory = trajectory;
            _distance = trajectory.Distance;
            _speed = speed;
            _damage = damage;
            ShouldBeDestroyed = false;
        }

        public void AddPassedTime(float deltaTime)
        {
            _passedTime += deltaTime;
            float ratio = Math.Clamp(_passedTime * _speed / _distance, 0, 1);
            _transform.SetPosition(_trajectory.Evaluate(ratio));
            
            if (ratio >= 1)
                ShouldBeDestroyed = true;
        }

        public void Hit(IDamageable damageable)
        {
            damageable.TakeDamage(_damage);
            ShouldBeDestroyed = true;
        }
    }
}