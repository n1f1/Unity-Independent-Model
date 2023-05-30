using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using Model.Characters.Player;
using Simulation;
using Simulation.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class ClientPlayerFactory : IPlayerFactory
    {
        private readonly ClientPlayerSimulationViewFactory _playerSimulationViewFactory;
        private readonly IViewInitializer<IPlayerView> _viewInitializer;
        private readonly IPlayerWithViewFactory _playerFactory;
        private readonly ClientPlayerSimulationInitializer _simulationInitializer;

        public ClientPlayerFactory(ClientPlayerSimulationViewFactory playerSimulationViewFactory,
            IViewInitializer<IPlayerView> viewInitializer, IPlayerWithViewFactory playerFactory,
            ClientPlayerSimulationInitializer clientPlayerSimulationInitializer)
        {
            _simulationInitializer = clientPlayerSimulationInitializer;
            _playerFactory = playerFactory;
            _viewInitializer = viewInitializer;
            _playerSimulationViewFactory = playerSimulationViewFactory;
        }

        public Player CreatePlayer(PlayerData playerData)
        {
            (IPlayerView, IPlayerSimulation, SimulationObject) created = _playerSimulationViewFactory.Create();

            _viewInitializer.InitializeView(created.Item1);

            FakeHealthView fakeHealthView = new FakeHealthView(created.Item1.HealthView);
            created.Item1.HealthView = fakeHealthView;

            Player player = _playerFactory.Create(playerData, created.Item1);

            _simulationInitializer.InitializeSimulation(player, created.Item2, created.Item3, created.Item1,
                fakeHealthView);
            created.Item3.AddUpdatable(fakeHealthView);

            return player;
        }
    }
}