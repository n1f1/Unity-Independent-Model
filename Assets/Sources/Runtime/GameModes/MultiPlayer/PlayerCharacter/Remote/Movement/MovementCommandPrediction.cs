using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using Model.Characters;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Movement
{
    public class MovementCommandPrediction : IMovementCommandPrediction
    {
        private readonly float _serverFixedDeltaTime;
        private readonly float _roundTripTime;
        private Vector3 _nextPosition;
        private MoveCommand _lastCommand;
        private float _timeSinceLastPacket;
        private Vector3 _currentPosition;
        private float _rollbackDelay = 0.02f;

        public MovementCommandPrediction(float roundTripTime, float serverFixedDeltaTime)
        {
            _serverFixedDeltaTime = serverFixedDeltaTime;
            _roundTripTime = roundTripTime + 0.01f;
        }

        public void PredictNextPacket(MoveCommand newCommand)
        {
            _lastCommand = newCommand;
            _timeSinceLastPacket = 0f;
            _nextPosition = newCommand.Position + newCommand.Acceleration * newCommand.Movement.Speed * _roundTripTime;
            _currentPosition = newCommand.Movement.Position;
        }

        public void Simulate(float deltaTime, CharacterMovement playerCharacterMovement)
        {
            if (_lastCommand == null)
                return;

            _timeSinceLastPacket += deltaTime;

            if (_timeSinceLastPacket > _serverFixedDeltaTime + _rollbackDelay)
                Rollback(deltaTime, playerCharacterMovement);
            else
                Predict(playerCharacterMovement);
        }

        private void Predict(CharacterMovement playerCharacterMovement)
        {
            playerCharacterMovement.Move(Vector3.Lerp(_currentPosition, _nextPosition,
                _timeSinceLastPacket / _serverFixedDeltaTime));
        }

        private void Rollback(float deltaTime, CharacterMovement playerCharacterMovement)
        {
            if (playerCharacterMovement.Position != _lastCommand.Position)
                MoveTowards(deltaTime, playerCharacterMovement, _lastCommand.Position);
        }

        private void MoveTowards(float deltaTime, CharacterMovement movement, Vector3 nextPosition)
        {
            Vector3 direction = Vector3.Normalize(nextPosition - movement.Position);

            if (Vector3.Distance(nextPosition, movement.Position) < movement.Speed * deltaTime)
                movement.Move(nextPosition);
            else
                movement.Move(direction, deltaTime);
        }
    }
}