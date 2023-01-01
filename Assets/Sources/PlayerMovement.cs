using Model;
using UnityEngine;

internal class PlayerMovement : MonoBehaviour, ISimulation
{
    private CharacterMovement _characterMovement;
    private IMovementInput _movementInput;

    public PlayerMovement Initialize(CharacterMovement characterMovement, IMovementInput movementInput)
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