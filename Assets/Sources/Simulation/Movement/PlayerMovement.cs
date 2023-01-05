﻿using System;
using Model;
using Model.Characters;
using Simulation.Input;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Simulation.Movement
{
    internal class PlayerMovement : MonoBehaviour, ISimulation
    {
        private IMovable _characterMovement;
        private IMovementInput _movementInput;

        public PlayerMovement Initialize(IMovable characterMovement, IMovementInput movementInput)
        {
            _movementInput = movementInput ?? throw new ArgumentException();
            _characterMovement = characterMovement ?? throw new ArgumentException();

            return this;
        }

        public void UpdatePassedTime(float deltaTime)
        {
            Vector3 input = new Vector3(_movementInput.X, 0, _movementInput.Y);

            if (input != Vector3.Zero)
                _characterMovement.Move(Vector3.Normalize(input) * deltaTime);
        }
    }
}