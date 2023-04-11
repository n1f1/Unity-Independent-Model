using Model;
using Model.Characters;
using Model.Characters.Shooting;
using Simulation;
using Simulation.Input;
using Simulation.Movement;
using Simulation.Shooting;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition.PlayerConstruction
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