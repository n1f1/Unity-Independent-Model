using System;
using System.Numerics;
using Model.Characters;
using Networking.Connection;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketSend;

namespace Server
{
    class ClientConnectionPlayerCreation : IClientConnection
    {
        private readonly ObjectReplicationPacketFactory _replicationPacketFactory;
        private readonly Room _room;
        private readonly GameLevel _game;
        
        public ClientConnectionPlayerCreation(ObjectReplicationPacketFactory replicationPacketFactory, Room room, GameLevel game)
        {
            _replicationPacketFactory = replicationPacketFactory ??
                                        throw new ArgumentNullException(nameof(replicationPacketFactory));
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Connect(Client client)
        {
            Player player = new NullPlayerFactory().CreatePlayer(new Vector3(1, 0, 1));

            INetworkPacket networkPacket = _replicationPacketFactory.Create(player);

            foreach (Client other in _room.Clients)
            {
                if (other.IsConnected)
                    other.Sender.SendPacket(networkPacket);
            }

            foreach (Player other in _game.Players)
                client.Sender.SendPacket(_replicationPacketFactory.Create(other));

            _game.Add(player);
        }
    }
}