using System.Collections;
using Simulation;

namespace GameModes.MultiPlayer
{
    public class ObjectToSimulationMap : IObjectToSimulationMap
    {
        private readonly Hashtable _objectToSimulation = new();

        public void RegisterNew<TSimulated>(TSimulated simulated, SimulationObject simulationObject)
        {
            _objectToSimulation.Add(simulated, simulationObject);
        }
        
        public SimulationObject Unregister<TSimulated>(TSimulated simulated)
        {
            SimulationObject simulation = Get(simulated);
            _objectToSimulation.Remove(simulated);

            return simulation;
        }

        public SimulationObject Get<TSimulated>(TSimulated simulated)
        {
            SimulationObject simulation = (SimulationObject) _objectToSimulation[simulated];

            return simulation;
        }
    }
}