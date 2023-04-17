using System;
using System.Net.Sockets;
using Networking.Connection;
using Networking.PacketReceive;
using Networking.PacketReceive.Replication;
using Server.Simulation;

namespace Server.Update
{
    internal class ServerUpdate : ITimeUpdate
    {
        private readonly IReplicationPacketRead _replicationPacketRead;
        private readonly NewClientsListener _newClientsListener;
        private readonly IPacketReceiver _packetReceiver;
        private readonly TcpListener _tcpListener;
        private readonly Room _room;
        private readonly GameSimulation _gameSimulation;

        public ServerUpdate(NewClientsListener newClientsListener, IReplicationPacketRead replicationPacketRead,
            Room room, TcpListener tcpListener, IPacketReceiver packetReceiver, GameSimulation gameSimulation)
        {
            _gameSimulation = gameSimulation ?? throw new ArgumentNullException(nameof(gameSimulation));
            _packetReceiver = packetReceiver ?? throw new ArgumentNullException(nameof(packetReceiver));
            _newClientsListener = newClientsListener ?? throw new ArgumentNullException(nameof(newClientsListener));
            _replicationPacketRead =
                replicationPacketRead ?? throw new ArgumentNullException(nameof(replicationPacketRead));
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _tcpListener = tcpListener ?? throw new ArgumentNullException(nameof(tcpListener));
        }

        public void AddPassedTime(float fixedTimeInMilliseconds)
        {
            _newClientsListener.ListenNewClients(_tcpListener);

            foreach (ServerClient client in _room.Clients)
            {
                if (client.IsConnected)
                    _packetReceiver.ReceivePackets(client.InputStream, _replicationPacketRead);
            }

            _gameSimulation.AddPassedTime(fixedTimeInMilliseconds);
        }
    }
}