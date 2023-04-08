using System;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using Model.Characters;
using Networking;
using Networking.PacketSend.ObjectSend;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
{
    public class ClientPlayerMovementCommandSender : IMovable
    {
        private readonly CharacterMovement _movement;
        private readonly INetworkObjectSender _networkObjectSender;
        private readonly NotReconciledMovementCommands _notReconciledMovementCommands;
        private short _id;

        public ClientPlayerMovementCommandSender(CharacterMovement movement, INetworkObjectSender networkObjectSender,
            NotReconciledMovementCommands movementCommands)
        {
            _notReconciledMovementCommands =
                movementCommands ?? throw new ArgumentNullException(nameof(movementCommands));
            _movement = movement ?? throw new ArgumentNullException(nameof(movement));
            _networkObjectSender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            Vector3 acceleration = direction;

            MoveCommand command = new MoveCommand(_movement, acceleration, deltaTime,
                _movement.GetPosition(direction, deltaTime), ++_id);

            _notReconciledMovementCommands.Add(command);
            command.Execute();
            _networkObjectSender.Send(command);
        }
    }
}