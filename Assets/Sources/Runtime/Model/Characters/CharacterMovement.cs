using System;
using System.Numerics;
using Model.SpatialObject;

namespace Model.Characters
{
    public class CharacterMovement : IMovable
    {
        private readonly Transform _transform;
        private readonly float _speed;

        public CharacterMovement(Transform transform, float speed)
        {
            _transform = transform ?? throw new ArgumentNullException();

            if (_speed < 0)
                throw new ArgumentOutOfRangeException();

            _speed = speed;
        }

        public Vector3 Position => _transform.Position;

        public void Move(Vector3 moveDelta) =>
            _transform.SetPosition(_transform.Position + moveDelta * _speed);

        public Vector3 GetPositionForDelta(Vector3 moveDelta) =>
            _transform.Position + moveDelta * _speed;

        public void MoveTo(Vector3 position) =>
            _transform.SetPosition(position);
    }
}