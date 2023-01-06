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
            _transform = transform ?? throw new ArgumentException();
            
            if (_speed < 0)
                throw new ArgumentException();
            
            _speed = speed;
        }

        public void Move(Vector3 moveDelta) => 
            _transform.SetPosition(_transform.Position + moveDelta * _speed);
    }
}