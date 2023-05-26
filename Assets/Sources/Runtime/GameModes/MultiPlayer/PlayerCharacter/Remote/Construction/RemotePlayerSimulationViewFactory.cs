using System;
using Model.Characters.Player;
using Simulation;
using Object = UnityEngine.Object;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    internal class RemotePlayerSimulationViewFactory
    {
        private readonly RemotePlayerTemplate _template;

        public RemotePlayerSimulationViewFactory(RemotePlayerTemplate template)
        {
            _template = template ? template : throw new ArgumentNullException(nameof(template));
        }

        public (IPlayerView, IRemotePlayerSimulation, SimulationObject) Create()
        {
            RemotePlayerTemplate playerTemplate = Object.Instantiate(_template);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);

            return (playerTemplate.PlayerView, playerTemplate.RemotePlayerSimulation, simulation);
        }
    }
}