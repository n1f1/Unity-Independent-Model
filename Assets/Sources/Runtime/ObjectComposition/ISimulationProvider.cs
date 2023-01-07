using SimulationObject;

namespace ObjectComposition
{
    internal interface ISimulationProvider<TSimulated>
    {
        public SimulationObject<TSimulated> CreateSimulationObject();
        void InitializeSimulation(SimulationObject<TSimulated> simulation, TSimulated simulated);
    }
}