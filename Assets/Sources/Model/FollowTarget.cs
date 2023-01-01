using System.Numerics;

namespace Model
{
    public class FollowTarget
    {
        private readonly Transform _target;
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;

        public FollowTarget(Transform target, Transform transform, CharacterMovement characterMovement)
        {
            _transform = transform;
            _characterMovement = characterMovement;
            _target = target;
        }

        public void Follow(float deltaTime)
        {
            Vector3 direction = Vector3.Normalize(_target.Position - _transform.Position) * deltaTime;
            _characterMovement.Move(direction.X, direction.Z);
        }
    }
}