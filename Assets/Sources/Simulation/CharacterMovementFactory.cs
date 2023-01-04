using Model;
using UnityEngine;

namespace Simulation
{
    internal class CharacterMovementFactory : ISimulationFactory<PlayerMovement, IMovable>
    {
        public IUpdatable Create(IMovable simulable, GameObject gameObject)
        {
            return gameObject.AddComponent<PlayerMovement>()
                .Initialize(simulable, new AxisInput());
        }
    }
}