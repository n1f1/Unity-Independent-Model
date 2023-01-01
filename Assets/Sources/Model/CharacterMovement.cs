using System;
using System.Numerics;
using Simulation;

namespace Model
{
    public class CharacterMovement : IMovable
    {
        private readonly Transform _transform;
        private readonly float _speed;

        public CharacterMovement(Transform transform, float speed)
        {
            _transform = transform ?? throw new ArgumentException();
            _speed = speed;
        }

        public void Move(float x, float z)
        {
            Vector3 transformPosition = _transform.Position;
            _transform.SetPosition(transformPosition.X + x * _speed, transformPosition.Z + z * _speed);
        }
    }
}