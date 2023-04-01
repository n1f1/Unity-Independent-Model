using System;
using System.Numerics;
using Model.Characters;
using ObjectComposition;

namespace ClientNetworking
{
    public class MovementCommandSender : IMovable
    {
        private readonly CharacterMovement _movement;
        private readonly IObjectSender _objectSender;

        public MovementCommandSender(CharacterMovement movement, IObjectSender objectSender)
        {
            _movement = movement ?? throw new ArgumentNullException(nameof(movement));
            _objectSender = objectSender ?? throw new ArgumentNullException(nameof(objectSender));
        }

        public void Move(Vector3 moveDelta)
        {
            MoveCommand command = new MoveCommand(_movement, moveDelta);
            _objectSender.Send(command);
        }
    }
}