using System;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using Model.Characters.Player;
using Simulation;
using Object = UnityEngine.Object;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    internal class RemotePlayerSimulationFactory : ISimulationFactory<IRemotePlayerSimulation>
    {
        private readonly RemotePlayerTemplate _template;

        public RemotePlayerSimulationFactory(RemotePlayerTemplate template)
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