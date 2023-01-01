namespace Model
{
    public class Enemy
    {
        private readonly FollowTarget _followTarget;

        public Enemy(IPositionView positionView, Transform target)
        {
            Transform transform = new Transform(positionView);
            transform.SetPosition(5, 5);
            _followTarget = new FollowTarget(target, transform, new CharacterMovement(transform, 4f));
        }

        public FollowTarget FollowTarget => _followTarget;
    }
}