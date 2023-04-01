using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientNetworking;
using ClientNetworking.NetworkingTypesConfigurations;
using JetBrains.Annotations;
using Networking.Packets;
using Networking.PacketSender;
using Networking.Replication;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using ObjectComposition;
using UnityEngine;


class GameClient
{
    private static int _id;
    public static IObjectSender ObjectSender;
    private readonly IInputStream _inputStream;
    private readonly IOutputStream _outputStream;
    private static IReplicationPacketRead _replicationPacketRead;
    private static NetworkStream _networkStream;

    public GameClient(IInputStream inputStream, IOutputStream outputStream)
    {
        _inputStream = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
        _outputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
    }

    public void CreateReplicator(Dictionary<Type, object> receivers,
        IEnumerable<(Type, object)> deserializationValues, TypeIdConversion typeId)
    {
        Dictionary<Type, IDeserialization<object>> deserialization = new();
        deserialization.PopulateDictionary(deserializationValues);

        _replicationPacketRead = new ReplicationPacketRead(new CreationReplicator(typeId, deserialization,
            new ReceivedReplicatedObjectMatcher(receivers)));

        Debug.Log("Create replicator");
    }

    public void CreateNetworkSending(IEnumerable<(Type, object)> serializationValues, TypeIdConversion typeId)
    {
        Dictionary<Type, object> serialization = new Dictionary<Type, object>();
        serialization.PopulateDictionary(serializationValues);

        INetworkPacketSender networkPacketSender = new SendingPacketsDebug(new NetworkPacketSender(_outputStream));

        ObjectReplicationPacketFactory replicationPacketFactory =
            new ObjectReplicationPacketFactory(serialization, typeId);

        ObjectSender = new ObjectSender(replicationPacketFactory, networkPacketSender);
        
        Debug.Log("Create sender");
    }

    public void ReceivePackets()
    {
        if (_inputStream.NotEmpty() == false)
            return;

        int readInt32 = _inputStream.ReadInt32();
        PacketType packetType = (PacketType) readInt32;

        Debug.Log("Received " + packetType + " time: " + DateTime.Now.TimeOfDay);

        switch (packetType)
        {
            case PacketType.ReplicationData:
                _replicationPacketRead.ProcessReplicationPacket(_inputStream);
                break;
            case PacketType.Handshake:
                GetHandshake(_inputStream);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    private static void GetHandshake(IInputStream inputStream)
    {
        _id = inputStream.ReadInt32();
        Debug.Log("Connected! client id: " + _id);
    }
}