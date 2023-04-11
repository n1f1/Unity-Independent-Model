using Model.Characters;
using Model.Characters.Player;
using Simulation.Input;
using UnityEngine;

namespace Simulation.Characters.Player
{
    public class PlayerSimulation : MonoBehaviour, IPlayerSimulation
    {
        public ISimulation<IMovable> Movable { get; set; }
        public ISimulation<ICharacterShooter> CharacterShooter { get; set; }

        protected virtual void Awake()
        {
            Movable = gameObject.AddComponent<PlayerMovement>().Initialize(new AxisInput());
            CharacterShooter = gameObject.AddComponent<PlayerShooter>();
        }
    }
}