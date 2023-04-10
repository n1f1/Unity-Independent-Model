using Simulation.Pool;
using UnityEngine;

namespace Simulation
{
    public class SimulationObject : SimulationObject<GameObject>, IPoolable
    {
        public SimulationObject(GameObject template) : base(template)
        {
            
        }

        public void Enable()
        {
            Template.SetActive(true);
        }

        public void Disable()
        {
            Template.SetActive(false);
        }
    }
}