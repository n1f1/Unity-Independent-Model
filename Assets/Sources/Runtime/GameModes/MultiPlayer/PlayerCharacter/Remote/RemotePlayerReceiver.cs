using System;
using Model.Characters.Player;
using Networking.PacketReceive;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    public class RemotePlayerReceiver : IReplicatedObjectReceiver<Player>
    {
        private IObjectToSimulationMap _objectToSimulationMap;

        public RemotePlayerReceiver(IObjectToSimulationMap objectToSimulation)
        {
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void Receive(Player createdObject)
        {
        }
    }
}