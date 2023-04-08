using GameModes.MultiPlayer.PlayerCharacter.Common;
using Model.Characters;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    public interface IMovementCommandPrediction
    {
        void PredictNextPacket(MoveCommand newCommand);
        void Simulate(float deltaTime, CharacterMovement playerCharacterMovement);
    }
}