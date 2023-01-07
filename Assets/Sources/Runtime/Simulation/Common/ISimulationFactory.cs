using Model;
using UnityEngine;

namespace Simulation.Common
{
    public interface ISimulationFactory<TSimulated>
    {
        public ISimulation<TSimulated> Create(TSimulated simulated, GameObject gameObject);
    }
}