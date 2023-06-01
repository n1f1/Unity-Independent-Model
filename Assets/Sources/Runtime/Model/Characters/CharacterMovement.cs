using System;
using System.Numerics;
using Model.SpatialObject;

namespace Model.Characters
{
    public class CharacterMovement : IMovable
    {
        private readonly Transform _transform;

        public CharacterMovement(Transform transform, float speed)
        {
            _transform = transform ?? throw new ArgumentNullException(nameof(transform));

            if (speed < 0)
                throw new ArgumentOutOfRangeException(nameof(speed));

            Speed = speed;
        }

        public Vector3 Position => _transform.Position;
        public float Speed { get; }

        public void Move(Vector3 direction, float deltaTime) =>
            _transform.SetPosition(_transform.Position + direction * deltaTime * Speed);

        public void Move(Vector3 position) =>
            _transform.SetPosition(position);

        public Vector3 GetPosition(Vector3 direction, float time) =>
            _transform.Position + direction * time * Speed;
    }
}