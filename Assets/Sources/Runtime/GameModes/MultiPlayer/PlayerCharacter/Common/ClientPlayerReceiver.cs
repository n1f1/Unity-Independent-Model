using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using Networking.Common.PacketReceive;
using Simulation;
using Simulation.Infrastructure;
using UnityEngine.PlayerLoop;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class ClientPlayerReceiver : IReplicatedObjectReceiver<ClientPlayer>
    {
        private readonly IObjectToSimulationMap _objectToSimulationMap;
        private readonly ClientPlayerSimulation _simulationClientPlayer;

        private bool _received;

        public ClientPlayerReceiver(IObjectToSimulationMap objectToSimulation,
            ClientPlayerSimulation simulation)
        {
            _simulationClientPlayer = simulation ?? throw new ArgumentNullException(nameof(simulation));
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void Receive(ClientPlayer createdObject)
        {
            if (_received)
                throw new InvalidOperationException();

            SimulationObject playerSimulation = _objectToSimulationMap.Get(createdObject.Player);
            _simulationClientPlayer.SetClientPlayerSimulation(createdObject.Player, playerSimulation);
            _received = true;
        }
    }
}