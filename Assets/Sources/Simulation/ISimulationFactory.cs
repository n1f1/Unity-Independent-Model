using Model;
using UnityEngine;

namespace Simulation
{
    public interface ISimulationFactory<out TSimulation, TSimulable> where TSimulation : ISimulation<TSimulable>
    {
        public IUpdatable Create(TSimulable simulable, GameObject gameObject);
    }
}