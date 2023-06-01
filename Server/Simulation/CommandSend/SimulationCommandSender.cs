using System;
using System.Collections.Generic;
using Networking.Common.PacketSend;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Server;

namespace Server.Simulation.CommandSend
{
    internal class SimulationCommandSender<TCommand> : ICommandSender<TCommand>
    {
        private readonly ObjectReplicationPacketFactory _replicationPacketFactory;
        private readonly Stack<TCommand> _commands = new();

        public SimulationCommandSender(ObjectReplicationPacketFactory factory)
        {
            _replicationPacketFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public void Send(TCommand command)
        {
            _commands.Push(command);
        }

        public void Send(Room room)
        {
            while (_commands.Count > 0)
            {
                TCommand replicatingObject = _commands.Pop();
                INetworkPacket packet = _replicationPacketFactory.Create(replicatingObject);

                foreach (ServerClient client in room.Clients)
                    client.Sender.SendPacket(packet);
            }
        }
    }
}