using System;
using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.Player;
using Networking;
using Networking.Connection;
using Networking.PacketReceive.Replication.ObjectCreationReplication;

namespace Server.Client
{
    internal class FireCommandHandler : ICommandHandler<FireCommand>
    {
        private readonly Player _player;
        private readonly List<FireCommand> _fireCommands = new();
        
        public FireCommandHandler(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public bool Dirty { get; private set; }

        public void AddCommand(FireCommand command)
        {
            _fireCommands.Add(command);
            Dirty = true;
        }

        public void ProcessReceivedCommands()
        {
            foreach (FireCommand fireCommand in _fireCommands)
            {
                fireCommand.Execute();

                if (fireCommand.Succeeded) 
                    fireCommand.Bullet.UpdateTime(NetworkConstants.ServerFixedDeltaTime / 2f);
            }
        }

        public void SendOutgoingCommands(Room room, ObjectReplicationPacketFactory objectReplicationPacketFactory)
        {
            foreach (FireCommand fireCommand in _fireCommands)
            {
                if (fireCommand.Succeeded)
                    room.Send(objectReplicationPacketFactory.Create(fireCommand));
            }

            _fireCommands.Clear();
            Dirty = false;
        }
    }
}