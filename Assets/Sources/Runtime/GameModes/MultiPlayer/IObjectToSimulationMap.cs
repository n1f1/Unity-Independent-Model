using Simulation;

namespace GameModes.MultiPlayer
{
    public interface IObjectToSimulationMap
    {
        void RegisterNew<TSimulated>(TSimulated simulated, SimulationObject<TSimulated> simulationObject);
        SimulationObject<TSimulated> Unregister<TSimulated>(TSimulated simulated);
        SimulationObject<TSimulated> Get<TSimulated>(TSimulated simulated);
    }
}