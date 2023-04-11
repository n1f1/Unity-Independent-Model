namespace Simulation.Infrastructure
{
    public interface IObjectToSimulationMap
    {
        void RegisterNew<TSimulated>(TSimulated simulated, SimulationObject simulationObject);
        SimulationObject Unregister<TSimulated>(TSimulated simulated);
        SimulationObject Get<TSimulated>(TSimulated simulated);
    }
}