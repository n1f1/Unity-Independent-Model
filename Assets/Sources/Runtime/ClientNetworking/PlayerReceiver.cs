using System;
using Model.Characters;
using Networking;
using UnityEngine;

namespace ClientNetworking
{
    public class PlayerReceiver : IReplicatedObjectReceiver<Player>
    {
        private bool _received;
        private Game _gameLevel;

        public PlayerReceiver(Game gameLevel)
        {
            _gameLevel = gameLevel;
        }

        public void Receive(Player player)
        {
            if(_received)
                return;
            
            _received = true;
            _gameLevel.Add(player);
            Debug.Log("Receive " + player + "!");
        }
    }
}