using System;
using System.Numerics;
using Model.Characters;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class MoveCommand : ICommand
    {
        public MoveCommand(CharacterMovement movement, Vector3 acceleration, float deltaTime, Vector3 position, short id)
        {
            if (deltaTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime));
            
            DeltaTime = deltaTime;
            Acceleration = acceleration;
            Movement = movement ?? throw new ArgumentNullException(nameof(movement));
            Position = position;
            ID = id;
        }
        
        public CharacterMovement Movement { get; }
        public Vector3 Acceleration { get; }
        public Vector3 Position { get; }
        public float DeltaTime { get; }
        public short ID { get; }

        public void Execute()
        {
            Movement.Move(Position);
        }
    }
}