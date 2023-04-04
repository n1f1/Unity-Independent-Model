using System;
using System.Collections.Generic;
using Networking.PacketSender;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using ObjectComposition;

namespace MultiPlayer
{
    public class StreamObjectSender : INetworkObjectSender
    {
        private readonly NetworkObjectSender _sender;

        public StreamObjectSender(IEnumerable<(Type, object)> serializationValues, TypeIdConversion typeId,
            IOutputStream networkOutputStream)
        {
            Dictionary<Type, object> serialization = new Dictionary<Type, object>();
            serialization.PopulateDictionary(serializationValues);

            INetworkPacketSender networkPacketSender =
                new SendingPacketsDebug(new NetworkPacketSender(networkOutputStream));

            ObjectReplicationPacketFactory replicationPacketFactory =
                new ObjectReplicationPacketFactory(serialization, typeId);

            _sender = new NetworkObjectSender(replicationPacketFactory, networkPacketSender);
        }

        public void Send<TType>(TType sent) => 
            _sender.Send(sent);
    }
}