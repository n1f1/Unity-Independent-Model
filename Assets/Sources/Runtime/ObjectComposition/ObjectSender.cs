using System;
using Networking.PacketSender;
using Networking.Replication.ObjectCreationReplication;
using UnityEngine;

namespace ObjectComposition
{
    public class NetworkObjectSender : INetworkObjectSender
    {
        private readonly ObjectReplicationPacketFactory _replicationPacketFactory;
        private readonly INetworkPacketSender _networkPacketSender;

        public NetworkObjectSender(ObjectReplicationPacketFactory packetFactory,
            INetworkPacketSender networkPacketSender)
        {
            _replicationPacketFactory = packetFactory ?? throw new ArgumentNullException(nameof(packetFactory));
            _networkPacketSender = networkPacketSender ?? throw new ArgumentNullException(nameof(networkPacketSender));
        }

        public void Send<TType>(TType sent)
        {
            INetworkPacket packet = _replicationPacketFactory.Create(sent);
            _networkPacketSender.SendPacket(packet);
        }
    }
}