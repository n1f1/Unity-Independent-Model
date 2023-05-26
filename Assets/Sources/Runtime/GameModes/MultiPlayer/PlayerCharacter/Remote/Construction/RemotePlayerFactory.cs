using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Construction;
using Model.Characters.Player;
using Simulation;

namespace GameModes.MultiPlayer
{
    internal class RemotePlayerFactory : IPlayerFactory
    {
        private readonly RemotePlayerSimulationViewFactory _playerSimulationViewFactory;
        private readonly IViewInitializer<IPlayerView> _viewInitializer;
        private readonly IPlayerWithViewFactory _playerFactory;
        private readonly RemotePlayerSimulationInitializer _simulationInitializer;

        public RemotePlayerFactory(RemotePlayerSimulationViewFactory playerSimulationViewFactory,
            IViewInitializer<IPlayerView> viewInitializer, IPlayerWithViewFactory playerFactory,
            RemotePlayerSimulationInitializer remotePlayerSimulationInitializer)
        {
            _simulationInitializer = remotePlayerSimulationInitializer;
            _playerFactory = playerFactory;
            _viewInitializer = viewInitializer;
            _playerSimulationViewFactory = playerSimulationViewFactory;
        }

        public Player CreatePlayer(Vector3 position)
        {
            (IPlayerView, IRemotePlayerSimulation, SimulationObject) created = _playerSimulationViewFactory.Create();
            
            _viewInitializer.InitializeView(created.Item1);

            Player player = _playerFactory.Create(position, created.Item1);

            _simulationInitializer.InitializeSimulation(player, created.Item2, created.Item3, created.Item1);
            
            return player;
        }
    }
}