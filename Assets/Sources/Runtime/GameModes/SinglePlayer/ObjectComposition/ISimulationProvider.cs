using Simulation;

namespace GameModes.SinglePlayer.ObjectComposition
{
    internal interface ISimulationProvider<TSimulated>
    {
        public SimulationObject CreateSimulationObject();
        void InitializeSimulation(SimulationObject simulation, TSimulated simulated);
    }
}