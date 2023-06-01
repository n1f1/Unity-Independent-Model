using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using Model.Characters.Player;
using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
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

        public Player CreatePlayer(PlayerData playerData)
        {
            (IPlayerView, IRemotePlayerSimulation, SimulationObject) created = _playerSimulationViewFactory.Create();

            _viewInitializer.InitializeView(created.Item1);

            FakeHealthView fakeHealthView = new FakeHealthView(created.Item1.HealthView);
            created.Item1.HealthView = fakeHealthView;

            Player player = _playerFactory.Create(playerData, created.Item1);

            _simulationInitializer.InitializeSimulation(player, created.Item2, created.Item3,
                fakeHealthView);
            created.Item3.AddUpdatable(fakeHealthView);

            return player;
        }
    }
}