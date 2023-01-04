using Model;
using UnityEngine;

namespace Simulation
{
    internal class ShooterFactory: ISimulationFactory<PlayerShooter, CharacterShooter>
    {
        public IUpdatable Create(CharacterShooter simulable, GameObject gameObject)
        {
            return gameObject.AddComponent<PlayerShooter>()
                .Initialize(simulable);
        }
    }
}