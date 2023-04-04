using System;
using System.Numerics;
using Model.Characters;
using ObjectComposition;

namespace ClientNetworking
{
    public class MovementCommandSender : IMovable
    {
        private readonly CharacterMovement _movement;
        private readonly INetworkObjectSender _networkObjectSender;

        public MovementCommandSender(CharacterMovement movement, INetworkObjectSender networkObjectSender)
        {
            _movement = movement ?? throw new ArgumentNullException(nameof(movement));
            _networkObjectSender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public void Move(Vector3 moveDelta)
        {
            MoveCommand command = new MoveCommand(_movement, moveDelta);
            _networkObjectSender.Send(command);
        }
    }
}