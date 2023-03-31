using System;
using ClientNetworking;
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
using Object = UnityEngine.Object;

namespace ObjectComposition
{
    public class PlayerSimulationProvider
    {
        private readonly IViewFactory<IPositionView> _positionViewFactory;
        private readonly IViewFactory<IHealthView> _healthViewFactory;
        private readonly GameObject _playerTemplate;

        public PlayerSimulationProvider(GameObject playerTemplate, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory)
        {
            _playerTemplate = playerTemplate ? playerTemplate : throw new ArgumentNullException();
            _healthViewFactory = healthViewFactory ?? throw new ArgumentNullException();
            _positionViewFactory = positionViewFactory ?? throw new ArgumentNullException();
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

        public void InitializeSimulation(SimulationObject<Player> simulation, Player simulated, IMovable movable)
        {
            simulation.AddSimulation(simulation.GameObject.AddComponent<PlayerMovement>()
                .Initialize(new AxisInput()));

            simulation.AddSimulation(simulation.GameObject.AddComponent<PlayerShooter>());

            ISimulation<IMovable> movableSimulation = simulation.GetSimulation<IMovable>();
            movableSimulation.Initialize(movable);
            simulation.RegisterUpdatable(movableSimulation);

            ISimulation<CharacterShooter> characterShooter = simulation.GetSimulation<CharacterShooter>();
            characterShooter.Initialize(simulated.CharacterShooter);
            simulation.RegisterUpdatable(characterShooter);
        }
    }
}