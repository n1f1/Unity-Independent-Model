using System;
using GameMenu;
using Model.Characters;
using MultiPlayer;
using Networking;
using UnityEngine;

namespace ClientNetworking
{
    public class PlayerReceiver : IReplicatedObjectReceiver<Player>
    {
        private readonly IObjectToSimulationMap _objectToSimulationMap;
        private readonly PlayerClient _clientPlayer;
        
        private bool _received;

        public PlayerReceiver(IObjectToSimulationMap objectToSimulation, PlayerClient clientPlayer)
        {
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void Receive(Player createdObject)
        {
            if(_received)
                return;
            
            _clientPlayer.SetClientPlayerSimulation(createdObject, _objectToSimulationMap.Get(createdObject));
            _received = true;
            Debug.Log("Receive " + createdObject + "!");
        }
    }
}