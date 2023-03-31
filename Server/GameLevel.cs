using System.Collections.Generic;
using Model.Characters;

namespace Server
{
    internal class GameLevel
    {
        private readonly List<Player> _players = new();
        public IEnumerable<Player> Players => _players;

        public void Add(Player player)
        {
            _players.Add(player);
        }
    }
}