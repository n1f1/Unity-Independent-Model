using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using Networking;
using Networking.PacketReceive;
using ObjectComposition;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class PlayerReceiver : IReplicatedObjectReceiver<Model.Characters.Player>
    {
        private readonly IObjectToSimulationMap _objectToSimulationMap;
        private readonly PlayerClient _clientPlayer;
        
        private bool _received;

        public PlayerReceiver(IObjectToSimulationMap objectToSimulation, PlayerClient clientPlayer)
        {
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void Receive(Model.Characters.Player createdObject)
        {
            if(_received)
                return;
            
            _clientPlayer.SetClientPlayerSimulation(createdObject, _objectToSimulationMap.Get(createdObject));
            _received = true;
            Debug.Log("Receive " + createdObject + "!");
        }
    }
}