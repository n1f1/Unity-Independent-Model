namespace Model
{
    public class Player
    {
        private readonly CharacterMovement _characterMovement;
        private readonly Transform _transform;

        public Player(IPositionView positionView)
        {
            _transform = new Transform(positionView);
            _characterMovement = new CharacterMovement(_transform, 5f);
        }

        public CharacterMovement CharacterMovement => _characterMovement;
        public Transform Transform => _transform;
    }
}