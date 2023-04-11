using System;
using Model.Characters;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Movement
{
    public class RemotePlayerMovementPrediction
    {
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly CharacterMovement _playerCharacterMovement;

        public RemotePlayerMovementPrediction(IMovementCommandPrediction prediction,
            CharacterMovement movement)
        {
            _playerCharacterMovement = movement ?? throw new ArgumentNullException(nameof(movement));
            _movementCommandPrediction = prediction ?? throw new ArgumentNullException(nameof(prediction));
        }

        public void UpdateTime(float deltaTime)
        {
            _movementCommandPrediction.Simulate(deltaTime, _playerCharacterMovement);
        }
    }
}