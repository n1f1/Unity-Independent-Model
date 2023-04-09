using System.Collections.Generic;
using Model.Characters;

namespace Server
{
    internal class PlayerToClientMap
    {
        private Dictionary<Player, GameClient> _clients = new();
        
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