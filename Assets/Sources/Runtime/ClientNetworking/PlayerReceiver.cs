using System;
using Model.Characters;
using Networking;
using UnityEngine;

namespace ClientNetworking
{
    public class PlayerReceiver : IReplicatedObjectReceiver<Player>
    {
        private bool _received;
        private Game _game;

        public PlayerReceiver(Game game)
        {
            _game = game;
        }

        public void Receive(Player player)
        {
            if(_received)
                return;
            
            _received = true;
            _game.Add(player);
            Debug.Log("Receive " + player + "!");
        }
    }
}