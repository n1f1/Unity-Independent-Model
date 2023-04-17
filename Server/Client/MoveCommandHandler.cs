using System;
using System.Collections.Generic;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using Model.Characters.Player;
using Networking.Connection;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketSend;

namespace Server.Client
{
    internal class MoveCommandHandler : ICommandHandler<MoveCommand>
    {
        private readonly Player _player;
        private readonly Queue<MoveCommand> _moveCommands = new();
        private Vector3 _acceleration;
        private float _deltaTime;
        private short _id;

        public MoveCommandHandler(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public bool Dirty { get; private set; }

        public void AddCommand(MoveCommand command)
        {
            _moveCommands.Enqueue(command);
            Dirty = true;
        }

        public void ProcessReceivedCommands()
        {
            _deltaTime = 0;

            while (_moveCommands.TryDequeue(out MoveCommand command))
            {
                command.Execute();
                _acceleration = command.Acceleration;
                _deltaTime += command.DeltaTime;

                if (command.ID > _id)
                    _id = command.ID;
            }
        }

        public void SendOutgoingCommands(Room room, ObjectReplicationPacketFactory objectReplicationPacketFactory)
        {
            MoveCommand moveCommand =
                new MoveCommand(_player, _acceleration, _deltaTime, _player.Transform.Position, _id);

            INetworkPacket movePacket = objectReplicationPacketFactory.Create(moveCommand);

            room.Send(movePacket);
            Dirty = false;
        }
    }
}