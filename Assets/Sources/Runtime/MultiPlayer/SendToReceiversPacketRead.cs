using System;
using System.Collections.Generic;
using ClientNetworking;
using Networking.Replication;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;

namespace MultiPlayer
{
    public class SendToReceiversPacketRead : IReplicationPacketRead
    {
        private readonly ReplicationPacketRead _replicationPacketRead;

        public SendToReceiversPacketRead(Dictionary<Type, object> receivers,
            IEnumerable<(Type, object)> deserializationValues, TypeIdConversion typeId)
        {
            Dictionary<Type, IDeserialization<object>> deserialization = new();
            deserialization.PopulateDictionary(deserializationValues);

            _replicationPacketRead = new ReplicationPacketRead(new CreationReplicator(typeId, deserialization,
                new ReceivedReplicatedObjectMatcher(receivers)));
        }

        public void ProcessReplicationPacket(IInputStream inputStream) => 
            _replicationPacketRead.ProcessReplicationPacket(inputStream);
    }
}