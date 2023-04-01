using System;
using Networking.PacketSender;
using Networking.Replication.ObjectCreationReplication;
using UnityEngine;

namespace ObjectComposition
{
    public class ObjectSender : IObjectSender
    {
        private readonly ObjectReplicationPacketFactory _replicationPacketFactory;
        private readonly INetworkPacketSender _networkPacketSender;

        public ObjectSender(ObjectReplicationPacketFactory packetFactory, INetworkPacketSender networkPacketSender)
        {
            _replicationPacketFactory = packetFactory ?? throw new ArgumentNullException(nameof(packetFactory));
            _networkPacketSender = networkPacketSender ?? throw new ArgumentNullException(nameof(networkPacketSender));
        }

        public void Send<TType>(TType command)
        {
            INetworkPacket packet = _replicationPacketFactory.Create(command);
            Debug.Log(packet);
            _networkPacketSender.SendPacket(packet);
        }
    }
}