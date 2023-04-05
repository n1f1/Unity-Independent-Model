using System;
using Model.Characters;
using ObjectComposition;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace ClientNetworking
{
    public class MovementCommandSender : IMovable
    {
        private readonly CharacterMovement _movement;
        private readonly INetworkObjectSender _networkObjectSender;
        private readonly NotReconciledMovementCommands _notReconciledMovementCommands;

        public MovementCommandSender(CharacterMovement movement, INetworkObjectSender networkObjectSender,
            NotReconciledMovementCommands movementCommands)
        {
            _notReconciledMovementCommands =
                movementCommands ?? throw new ArgumentNullException(nameof(movementCommands));
            _movement = movement ?? throw new ArgumentNullException(nameof(movement));
            _networkObjectSender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public void Move(Vector3 moveDelta)
        {
            Vector3 position = _movement.GetPositionForDelta(moveDelta);
            MoveCommand command = new MoveCommand(_movement, position, DateTime.Now.TimeOfDay);
            _notReconciledMovementCommands.Add(command);
            Debug.Log(command.Position);
            _networkObjectSender.Send(command);
            command.Execute();
        }
    }
}