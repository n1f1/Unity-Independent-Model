using System;
using GameModes.SinglePlayer;
using Model.Characters.Player;
using Simulation;
using Simulation.Characters.Player;
using Object = UnityEngine.Object;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal class ClientPlayerSimulationViewFactory
    {
        private readonly SinglePlayerTemplate _template;

        public ClientPlayerSimulationViewFactory(SinglePlayerTemplate template)
        {
            _template = template ? template : throw new ArgumentNullException(nameof(template));
        }

        public (IPlayerView, IPlayerSimulation, SimulationObject) Create()
        {
            SinglePlayerTemplate playerTemplate = Object.Instantiate(_template);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);

            return (playerTemplate.PlayerView, playerTemplate.PlayerSimulation, simulation);
        }
    }
}