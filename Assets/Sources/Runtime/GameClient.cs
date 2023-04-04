using System;
using Networking;
using Networking.Packets;
using Networking.Replication;
using Networking.StreamIO;
using UnityEngine;

class GameClient : INetworkStreamRead
{
    private static int _id;
    private readonly IReplicationPacketRead _packetRead;

    public GameClient(IReplicationPacketRead packetRead)
    {
        _packetRead = packetRead ?? throw new ArgumentNullException(nameof(packetRead));
    }

    public void ReadNetworkStream(IInputStream inputStream)
    {
        Debug.Log(inputStream.NotEmpty());
        while (inputStream.NotEmpty())
        {
            int readInt32 = inputStream.ReadInt32();
            PacketType packetType = (PacketType) readInt32;

            Debug.Log("Received " + packetType + " time: " + DateTime.Now.TimeOfDay);

            switch (packetType)
            {
                case PacketType.ReplicationData:
                    _packetRead.ProcessReplicationPacket(inputStream);
                    break;
                case PacketType.Handshake:
                    GetHandshake(inputStream);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    private static void GetHandshake(IInputStream inputStream)
    {
        _id = inputStream.ReadInt32();
        Debug.Log("Connected! client id: " + _id);
    }
}