using System;
using System.Reflection;
using ClientNetworking;
using Networking;
using Networking.PacketSender;
using Networking.Replication.ObjectCreationReplication;

namespace Server
{
    internal class CommandsResendReceiver : IReplicatedObjectReceiver<ICommand>
    {
        private readonly ObjectReplicationPacketFactory _replicationPacketFactory;
        private readonly Room _room;

        public CommandsResendReceiver(Room room, ObjectReplicationPacketFactory replicationPacketFactory)
        {
            _replicationPacketFactory = replicationPacketFactory;
            _room = room ?? throw new ArgumentNullException(nameof(room));
        }

        public void Receive(ICommand createdObject)
        {
            foreach (Client client in _room.Clients)
            {
                if (client.IsConnected == false)
                    continue;

                Type replicatingObject = createdObject.GetType();
                Type factoryType = _replicationPacketFactory.GetType();
                MethodInfo methodInfo = factoryType.GetMethod(nameof(_replicationPacketFactory.Create))
                    .MakeGenericMethod(replicatingObject);
                INetworkPacket packet =
                    (INetworkPacket) methodInfo.Invoke(_replicationPacketFactory, new object[] {createdObject});

                client.Sender.SendPacket(packet);
            }
        }
    }
}