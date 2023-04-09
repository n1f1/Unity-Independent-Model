using System;
using System.Collections.Generic;
using Networking.Connection;

namespace Server
{
    internal class GameSimulation
    {
        private readonly List<GameClient> _gameClients = new();
        private Room _room;
        public IEnumerable<GameClient> GameClients => _gameClients;

        public void Add(GameClient player, Room room)
        {
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _gameClients.Add(player);
        }

        public void AddPassedTime(float time)
        {
            foreach (GameClient gameClient in _gameClients)
            {
                gameClient.Player.UpdateTime(time);
                gameClient.ProcessReceivedCommands();
                gameClient.SendOutgoingCommands(_room);
            }
        }
    }
}