using System;
using System.Numerics;
using Model.Characters;

namespace ClientNetworking
{
    public class MoveCommand : ICommand
    {
        public MoveCommand(CharacterMovement movement, Vector3 position, TimeSpan dateCreationTime)
        {
            CreationTime = dateCreationTime;
            Position = position;
            Movement = movement ?? throw new ArgumentNullException(nameof(movement));
        }
        
        public TimeSpan CreationTime { get; }
        public CharacterMovement Movement { get; }
        public Vector3 Position { get; }

        public void Execute()
        {
            Movement.MoveTo(Position);
        }
    }
}