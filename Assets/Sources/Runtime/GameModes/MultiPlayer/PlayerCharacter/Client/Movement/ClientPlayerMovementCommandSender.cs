using System;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using Model.Characters;
using Model.Characters.Player;
using Networking.Common.PacketSend.ObjectSend;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Movement
{
    public class ClientPlayerMovementCommandSender : IMovable
    {
        private readonly INetworkObjectSender _networkObjectSender;
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly Player _player;
        private short _id;

        public ClientPlayerMovementCommandSender(Player player, INetworkObjectSender networkObjectSender,
            NotReconciledCommands<MoveCommand> commands)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _notReconciledCommands = commands ?? throw new ArgumentNullException(nameof(commands));
            _networkObjectSender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }


        public void Move(Vector3 direction, float deltaTime)
        {
            Vector3 acceleration = direction;
            Vector3 position = _player.CharacterMovement.GetPosition(direction, deltaTime);

            MoveCommand command = new MoveCommand(_player, acceleration, deltaTime, position, ++_id);

            _notReconciledCommands.Add(command);
            command.Execute();
            _networkObjectSender.Send(command);
        }
    }
}