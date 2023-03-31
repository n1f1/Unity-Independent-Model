using System;
using System.Numerics;
using Model.Characters;

namespace ClientNetworking
{
    public class MoveCommand : ICommand
    {
        public MoveCommand(CharacterMovement movement, Vector3 moveDelta)
        {
            MoveDelta = moveDelta;
            Movement = movement ?? throw new ArgumentNullException(nameof(movement));
        }

        public CharacterMovement Movement { get; }
        public Vector3 MoveDelta { get; }

        public void Execute()
        {
            Movement.Move(MoveDelta);
        }
    }
}