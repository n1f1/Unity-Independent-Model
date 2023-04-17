using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using Model.Characters.Player;
using Networking.Common.PacketSend;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Server;
using Networking.Server.Connection;
using Server.Characters.ClientPlayer;
using Server.Simulation;

namespace Server.Client
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

        public void Connect(ServerClient serverClient)
        {
            Player player = _playerFactory.CreatePlayer(new Vector3(1, 0, 1));

            INetworkPacket playerPacket = _replicationPacketFactory.Create(player);
            INetworkPacket clientPlayerPacket = _replicationPacketFactory.Create(new ClientPlayer(player));

            foreach (ServerClient other in _room.Clients)
            {
                if (other.IsConnected)
                    other.Sender.SendPacket(other == serverClient ? clientPlayerPacket : playerPacket);
            }

            foreach (GameClient other in _game.GameClients)
                serverClient.Sender.SendPacket(_replicationPacketFactory.Create(other.Player));

            GameClient gameClient = new GameClient(player, _replicationPacketFactory);
            _playerToClientMap.Add(gameClient);
            _game.Add(gameClient);
        }
    }
}