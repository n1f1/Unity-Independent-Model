using Model;
using UnityEngine;

namespace Simulation
{
    internal class PlayerMovement : MonoBehaviour, ISimulation<IMovable>
    {
        private IMovable _characterMovement;
        private IMovementInput _movementInput;

        public PlayerMovement Initialize(IMovable characterMovement, IMovementInput movementInput)
        {
            _movementInput = movementInput;
            _characterMovement = characterMovement;
            return this;
        }

        public void UpdatePassedTime(float deltaTime)
        {
            _characterMovement.Move(deltaTime * _movementInput.X, deltaTime * _movementInput.Y);
        }
    }
}