using System.Collections;
using GameMenu;
using SimulationObject;

namespace ObjectComposition
{
    public class ObjectToSimulationMap : IObjectToSimulationMap
    {
        private readonly Hashtable _objectToSimulation = new();

        public void RegisterNew<TSimulated>(TSimulated simulated, SimulationObject<TSimulated> simulationObject)
        {
            _objectToSimulation.Add(simulated, simulationObject);
        }
        
        public SimulationObject<TSimulated> Unregister<TSimulated>(TSimulated simulated)
        {
            SimulationObject<TSimulated> simulation = Get(simulated);
            _objectToSimulation.Remove(simulated);

            return simulation;
        }

        public SimulationObject<TSimulated> Get<TSimulated>(TSimulated simulated)
        {
            SimulationObject<TSimulated> simulation = (SimulationObject<TSimulated>) _objectToSimulation[simulated];

            return simulation;
        }
    }
}