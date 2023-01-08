using Model;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;
using Simulation.Input;
using Simulation.Movement;
using Simulation.Shooting;
using SimulationObject;
using UnityEngine;
using View.Factories;

namespace ObjectComposition
{
    public class PlayerSimulationProvider : ISimulationProvider<Player>
    {
        private readonly IViewFactory<IPositionView> _positionViewFactory;
        private readonly IViewFactory<IHealthView> _healthViewFactory;
        private readonly GameObject _playerTemplate;

        public PlayerSimulationProvider(GameObject playerTemplate, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory)
        {
            _healthViewFactory = healthViewFactory;
            _positionViewFactory = positionViewFactory;
            _playerTemplate = playerTemplate;
        }

        public SimulationObject<Player> CreateSimulationObject()
        {
            GameObject player = Object.Instantiate(_playerTemplate);
            SimulationObject<Player> simulation = new SimulationObject<Player>(player);
            simulation.Add(_positionViewFactory.Create(player));
            simulation.Add(_healthViewFactory.Create(player));
            simulation.Add(player.GetComponentInChildren<IForwardAimView>());

            return simulation;
        }

        public void InitializeSimulation(SimulationObject<Player> simulation, Player simulated)
        {
            simulation.AddSimulation(simulation.GameObject.AddComponent<PlayerMovement>()
                .Initialize(new AxisInput()));
            
            simulation.AddSimulation(simulation.GameObject.AddComponent<PlayerShooter>());

            ISimulation<IMovable> movableSimulation = simulation.GetSimulation<IMovable>();
            movableSimulation.Initialize(simulated.CharacterMovement);
            simulation.RegisterUpdatable(movableSimulation);

            ISimulation<CharacterShooter> characterShooter = simulation.GetSimulation<CharacterShooter>();
            characterShooter.Initialize(simulated.CharacterShooter);
            simulation.RegisterUpdatable(characterShooter);
        }
    }
}