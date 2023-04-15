namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal interface ISimulationInitializer<in TSimulated, in TSimulation, in TSimulationObject>
    {
        void InitializeSimulation(TSimulated simulated, TSimulation simulation, TSimulationObject simulationObject);
    }
}