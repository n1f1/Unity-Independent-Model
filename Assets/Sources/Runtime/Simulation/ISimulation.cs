using Model;

namespace Simulation
{
    public interface ISimulation<in TSimulated> : IUpdatable
    {
        ISimulation<TSimulated> Initialize(TSimulated simulated);
    }
}