using Model;
using UnityEngine;

namespace Simulation.Common
{
    public interface ISimulationFactory<in TSimulated>
    {
        public IUpdatable Create(TSimulated simulated, GameObject gameObject);
    }
}