using System;
using Model;
using Model.Characters;
using Simulation.Input;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Simulation.Movement
{
    internal class PlayerMovement : MonoBehaviour, ISimulation<IMovable>
    {
        private IMovable _movable;
        private IMovementInput _movementInput;

        public PlayerMovement Initialize(IMovementInput movementInput)
        {
            _movementInput = movementInput ?? throw new ArgumentNullException();

            return this;
        }

        public void Initialize(IMovable movable)
        {
            _movable = movable ?? throw new ArgumentNullException();
        }

        public void UpdateTime(float deltaTime)
        {
            Vector3 input = new Vector3(_movementInput.X, 0, _movementInput.Y);

            if (input != Vector3.Zero)
                _movable.Move(Vector3.Normalize(input) * deltaTime);
        }
    }
}