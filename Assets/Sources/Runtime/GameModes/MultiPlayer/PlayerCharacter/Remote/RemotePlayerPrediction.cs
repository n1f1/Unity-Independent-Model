using System;
using Model.Characters;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    public class RemotePlayerPrediction
    {
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly CharacterMovement _playerCharacterMovement;

        public RemotePlayerPrediction(IMovementCommandPrediction movementCommandPrediction,
            CharacterMovement playerCharacterMovement)
        {
            _playerCharacterMovement = playerCharacterMovement ??
                                       throw new ArgumentNullException(nameof(playerCharacterMovement));
            _movementCommandPrediction = movementCommandPrediction ??
                                       throw new ArgumentNullException(nameof(movementCommandPrediction));
        }

        public void UpdateTime(float deltaTime)
        {
            _movementCommandPrediction.Simulate(deltaTime, _playerCharacterMovement);
        }
    }
}