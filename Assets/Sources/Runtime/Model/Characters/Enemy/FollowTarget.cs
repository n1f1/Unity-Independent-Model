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
            _transform = transform ?? throw new ArgumentException();
            _characterMovement = characterMovement ?? throw new ArgumentException();
            _target = target ?? throw new ArgumentException();
        }

        public void Follow(float deltaTime)
        {
            if (deltaTime <= 0)
                throw new ArgumentException();

            if(_target.Position == _transform.Position)
                return;
            
            Vector3 stopOffset = -_stoppingDistance * Vector3.Normalize(_target.Position - _transform.Position);
            Vector3 direction = _target.Position + stopOffset - _transform.Position;

            if (direction == Vector3.Zero)
                return;

            Vector3 moveDelta = Vector3.Normalize(direction) * deltaTime;
            _characterMovement.Move(moveDelta);
        }
    }
}