using System.Collections.Generic;
using Model.Characters.Player;

namespace Server.Client
{
    internal class PlayerToClientMap
    {
        private readonly Dictionary<Player, GameClient> _clients = new();

        public void Add(GameClient gameClient)
        {
            _clients.Add(gameClient.Player, gameClient);
        }

        public GameClient Get(Player player)
        {
            return _clients[player];
        }
    }
}