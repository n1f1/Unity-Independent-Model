using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using Simulation;
using Simulation.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    public interface IRemotePlayerSimulation : IPlayerSimulation
    {
        ISimulation<RemotePlayerMovementPrediction> PlayerMovePrediction { get; set; }
    }
}