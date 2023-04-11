using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using Networking.PacketReceive;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class ClientPlayerReceiver : IReplicatedObjectReceiver<ClientPlayer>
    {
        private readonly IObjectToSimulationMap _objectToSimulationMap;
        private readonly PlayerClient _clientPlayer;
        
        private bool _received;

        public ClientPlayerReceiver(IObjectToSimulationMap objectToSimulation, PlayerClient clientPlayer)
        {
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void Receive(ClientPlayer createdObject)
        {
            if(_received)
                return;
            
            _clientPlayer.SetClientPlayerSimulation(createdObject.Player, _objectToSimulationMap.Get(createdObject.Player));
            _received = true;
        }
    }
}