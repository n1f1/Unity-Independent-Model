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
        private readonly GameSimulation _game;
        private readonly PlayerToClientMap _playerToClientMap;
        private readonly ServerPlayerFactory _playerFactory;

        public ClientConnectionPlayerCreation(ObjectReplicationPacketFactory replicationPacketFactory, Room room,
            GameSimulation game, PlayerToClientMap playerToClientMap, ServerPlayerFactory serverPlayerFactory)
        {
            _playerFactory = serverPlayerFactory ?? throw new ArgumentNullException(nameof(serverPlayerFactory));
            _playerToClientMap = playerToClientMap ?? throw new ArgumentNullException(nameof(playerToClientMap));
            _replicationPacketFactory = replicationPacketFactory ??
                                        throw new ArgumentNullException(nameof(replicationPacketFactory));
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Connect(Client client)
        {
            Player player = _playerFactory.CreatePlayer(new Vector3(1, 0, 1));

            INetworkPacket networkPacket = _replicationPacketFactory.Create(player);

            foreach (Client other in _room.Clients)
            {
                if (other.IsConnected)
                    other.Sender.SendPacket(networkPacket);
            }

            foreach (GameClient other in _game.GameClients)
                client.Sender.SendPacket(_replicationPacketFactory.Create(other.Player));

            GameClient gameClient = new GameClient(player, _replicationPacketFactory);
            _playerToClientMap.Add(gameClient);
            _game.Add(gameClient, _room);
        }
    }
}