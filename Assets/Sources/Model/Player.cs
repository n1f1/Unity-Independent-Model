namespace Model
{
    public class Player
    {
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;
        private IDamageable _health;

        public Player(IPositionView positionView, IHealthView healthView)
        {
            _health = new Health(healthView);
            _transform = new Transform(positionView);
            _characterMovement = new CharacterMovement(_transform, 5f);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
        public IDamageable Health => _health;
    }
}