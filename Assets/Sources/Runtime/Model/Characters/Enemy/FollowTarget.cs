using System;
using System.Numerics;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class FollowTarget
    {
        private readonly Transform _target;
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private readonly float _stoppingDistance = 1;

        public FollowTarget(Transform target, Transform transform, CharacterMovement characterMovement)
        {
            _transform = transform ?? throw new ArgumentNullException();
            _characterMovement = characterMovement ?? throw new ArgumentNullException();
            _target = target ?? throw new ArgumentNullException();
        }

        public void Follow(float deltaTime)
        {
            if (deltaTime <= 0)
                throw new ArgumentOutOfRangeException();

            if (_target.Position != _transform.Position)
                Move(deltaTime);
        }

        private void Move(float deltaTime)
        {
            Vector3 direction = GetMoveDirection();

            if (direction == Vector3.Zero)
                return;

            _characterMovement.Move(Vector3.Normalize(direction), deltaTime);
        }

        private Vector3 GetMoveDirection()
        {
            Vector3 stopOffset = -_stoppingDistance * Vector3.Normalize(_target.Position - _transform.Position);
            Vector3 direction = _target.Position + stopOffset - _transform.Position;
            return direction;
        }
    }
}