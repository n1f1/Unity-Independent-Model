using Simulation.Pool;

namespace Simulation
{
    class PoolableSimulationObject<TTemplate> : SimulationObject<TTemplate>, IPoolable where TTemplate : IPoolable
    {
        public PoolableSimulationObject(TTemplate template) : base(template)
        {
        }

        public void Enable()
        {
            Template.Enable();
        }

        public void Disable()
        {
            Template.Disable();
        }
    }
}