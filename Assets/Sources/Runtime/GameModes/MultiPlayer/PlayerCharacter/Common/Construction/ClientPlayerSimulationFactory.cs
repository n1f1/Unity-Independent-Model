using GameModes.SinglePlayer;
using Model.Characters.Player;
using Simulation;
using Simulation.Characters.Player;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal class ClientPlayerSimulationFactory : ISimulationFactory<IPlayerSimulation>
    {
        private readonly SinglePlayerTemplate _template;

        public ClientPlayerSimulationFactory(SinglePlayerTemplate template)
        {
            _template = template;
        }

        public (IPlayerView, IPlayerSimulation, SimulationObject) Create()
        {
            SinglePlayerTemplate playerTemplate = Object.Instantiate(_template);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);

            return (playerTemplate.PlayerView, playerTemplate.PlayerSimulation, simulation);
        }
    }
}