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

            if (Speed < 0)
                throw new ArgumentOutOfRangeException();

            _speed = speed;
        }

        public Vector3 Position => _transform.Position;

        public float Speed => _speed;

        public void Move(Vector3 direction, float deltaTime) =>
            _transform.SetPosition(_transform.Position + direction * deltaTime * Speed);

        public void Move(Vector3 position)
        {
            _transform.SetPosition(position);
        }

        public Vector3 GetPosition(Vector3 direction, float time)
        {
            return _transform.Position + direction * time * Speed;
        }
    }
}