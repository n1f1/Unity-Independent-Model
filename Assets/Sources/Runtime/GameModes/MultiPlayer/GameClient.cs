using System;
using Networking;
using Networking.PacketReceive;
using Networking.PacketReceive.Replication;
using Networking.PacketSend.Packets;
using Networking.StreamIO;
using UnityEngine;

namespace GameModes.MultiPlayer
{
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
            while (inputStream.NotEmpty())
            {
                int readInt32 = inputStream.ReadInt32();
                PacketType packetType = (PacketType) readInt32;

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
}