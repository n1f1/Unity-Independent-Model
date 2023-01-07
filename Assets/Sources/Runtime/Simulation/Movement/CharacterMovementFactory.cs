using Model;
using Model.Characters;
using Simulation.Common;
using Simulation.Input;
using UnityEngine;

namespace Simulation.Movement
{
    internal class CharacterMovementFactory : ISimulationFactory<IMovable>
    {
        public ISimulation<IMovable> Create(IMovable simulated, GameObject gameObject)
        {
            return gameObject.AddComponent<PlayerMovement>()
                .Initialize(new AxisInput());
        }
    }
}