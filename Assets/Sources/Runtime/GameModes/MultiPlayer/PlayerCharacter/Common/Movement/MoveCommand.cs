using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using Model.Characters;
using Model.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Movement
{
    public class MoveCommand : ICommand, IReconciliationCommand
    {
        public MoveCommand(Player player, Vector3 acceleration, float deltaTime, Vector3 position, short id)
        {
            if (deltaTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime));

            Player = player ?? throw new ArgumentNullException(nameof(player));
            DeltaTime = deltaTime;
            Acceleration = acceleration;
            Position = position;
            ID = id;
        }

        public Player Player { get; }
        public Vector3 Acceleration { get; }
        public Vector3 Position { get; }
        public float DeltaTime { get; }
        public short ID { get; }
        public CharacterMovement Movement => Player.CharacterMovement;

        public void Execute()
        {
            Movement.Move(Position);
        }
    }
}