using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using Model.Characters.Player;
using Simulation;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class PlayerWithViewAndSimulationFactory<TSimulation> : IPlayerFactory
    {
        private readonly ISimulationInitializer<Player, TSimulation, SimulationObject> _simulationInitializer;
        private readonly IPlayerWithViewFactory _playerFactory;
        private readonly IViewInitializer<IPlayerView> _viewInitializer;
        private readonly ISimulationFactory<TSimulation> _simulationFactory;

        public PlayerWithViewAndSimulationFactory(ISimulationFactory<TSimulation> simulationFactory,
            IViewInitializer<IPlayerView> viewInitializer, IPlayerWithViewFactory playerFactory,
            ISimulationInitializer<Player, TSimulation, SimulationObject> simulationInitializer)
        {
            _simulationFactory = simulationFactory;
            _viewInitializer = viewInitializer;
            _playerFactory = playerFactory;
            _simulationInitializer = simulationInitializer;
        }

        public Player CreatePlayer(Vector3 position)
        {
            (IPlayerView, TSimulation, SimulationObject) simulation = _simulationFactory.Create();

            _viewInitializer.InitializeView(simulation.Item1);

            Player player = _playerFactory.Create(position, simulation.Item1);

            _simulationInitializer.InitializeSimulation(player, simulation.Item2, simulation.Item3);

            return player;
        }
    }
}