using Model.Characters.Player;
using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal interface ISimulationFactory<TSimulation>
    {
        (IPlayerView, TSimulation, SimulationObject) Create();
    }
}