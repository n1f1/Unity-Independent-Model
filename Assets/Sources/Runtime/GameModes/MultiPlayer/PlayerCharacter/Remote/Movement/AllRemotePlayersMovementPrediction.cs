using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using Model.Characters;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Movement
{
    public class AllRemotePlayersMovementPrediction : IMovementCommandPrediction
    {
        private readonly Dictionary<CharacterMovement, MovementCommandPrediction> _predictions = new();
        private readonly float _roundTripTime;
        private readonly float _serverFixedDeltaTime;

        public AllRemotePlayersMovementPrediction(float roundTripTime, float serverFixedDeltaTime)
        {
            _serverFixedDeltaTime = serverFixedDeltaTime;
            _roundTripTime = roundTripTime;
        }

        public void PredictNextPacket(MoveCommand newCommand)
        {
            AddIfNew(newCommand.Movement);
            _predictions[newCommand.Movement].PredictNextPacket(newCommand);
        }

        public void Simulate(float deltaTime, CharacterMovement playerCharacterMovement)
        {
            AddIfNew(playerCharacterMovement);
            _predictions[playerCharacterMovement].Simulate(deltaTime, playerCharacterMovement);
        }

        private void AddIfNew(CharacterMovement newCharacterMovement)
        {
            if (_predictions.ContainsKey(newCharacterMovement) == false)
            {
                var commandPrediction = new MovementCommandPrediction(_roundTripTime, _serverFixedDeltaTime);
                _predictions.Add(newCharacterMovement, commandPrediction);
            }
        }
    }
}