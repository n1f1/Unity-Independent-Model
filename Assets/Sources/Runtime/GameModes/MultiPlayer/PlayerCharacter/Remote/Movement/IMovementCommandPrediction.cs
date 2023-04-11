using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using Model.Characters;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Movement
{
    public interface IMovementCommandPrediction
    {
        void PredictNextPacket(MoveCommand newCommand);
        void Simulate(float deltaTime, CharacterMovement playerCharacterMovement);
    }
}