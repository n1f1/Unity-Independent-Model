using System;
using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Networking;
using Networking.Common;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Server;

namespace Server.Client
{
    internal class FireCommandHandler : ICommandHandler<FireCommand>
    {
        private readonly List<FireCommand> _fireCommands = new();

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