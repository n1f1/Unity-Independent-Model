using Model;
using Model.Characters.Shooting;
using Simulation.Common;
using UnityEngine;

namespace Simulation.Shooting
{
    internal class ShooterFactory : ISimulationFactory<CharacterShooter>
    {
        public IUpdatable Create(CharacterShooter simulated, GameObject gameObject)
        {
            return gameObject.AddComponent<PlayerShooter>()
                .Initialize(simulated);
        }
    }
}