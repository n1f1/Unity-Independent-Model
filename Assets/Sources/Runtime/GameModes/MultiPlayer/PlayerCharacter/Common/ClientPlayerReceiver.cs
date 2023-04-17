using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using Networking.Common.PacketReceive;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class ClientPlayerReceiver : IReplicatedObjectReceiver<ClientPlayer>
    {
        private readonly IObjectToSimulationMap _objectToSimulationMap;
        private readonly ClientPlayerSimulation _simulationClientPlayer;
        
        private bool _received;

        public ClientPlayerReceiver(IObjectToSimulationMap objectToSimulation, ClientPlayerSimulation simulationClientPlayer)
        {
            _simulationClientPlayer = simulationClientPlayer ?? throw new ArgumentNullException(nameof(simulationClientPlayer));
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void Receive(ClientPlayer createdObject)
        {
            if(_received)
                return;
            
            _simulationClientPlayer.SetClientPlayerSimulation(createdObject.Player, _objectToSimulationMap.Get(createdObject.Player));
            _received = true;
        }
    }
}