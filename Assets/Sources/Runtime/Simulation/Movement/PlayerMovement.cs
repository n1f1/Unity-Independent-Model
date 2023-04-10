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

        public ISimulation<IMovable> Initialize(IMovable enemyPlayerPrediction)
        {
            _movable = enemyPlayerPrediction ?? throw new ArgumentNullException();
            
            return this;
        }

        public void UpdateTime(float deltaTime)
        {
            Vector3 input = new Vector3(_movementInput.X, 0, _movementInput.Y);

            if (input.Length() > 1f)
                input = Vector3.Normalize(input);

            if (input != Vector3.Zero)
                _movable.Move(input, deltaTime);
        }
    }
}