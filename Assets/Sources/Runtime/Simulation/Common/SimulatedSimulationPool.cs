namespace Simulation.Common
{
    public class SimulatedSimulationPool<TObject> : KeyValueObjectPool<TObject, SimulationObject<TObject>>
    {
        public SimulatedSimulationPool(int capacity) : base(capacity)
        {
            
        }
    }
}