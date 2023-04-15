using System;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using Model.Characters.Player;
using Simulation;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    public class RemotePlayerSimulationInitializer : ISimulationInitializer<Player, IRemotePlayerSimulation, SimulationObject>
    {
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly UpdatableContainer _updatableContainer;

        public RemotePlayerSimulationInitializer(IObjectToSimulationMap objectToSimulationMapping, UpdatableContainer updatableContainer,
            IMovementCommandPrediction movementCommandPrediction)
        {
            _movementCommandPrediction = movementCommandPrediction;
            _updatableContainer = updatableContainer;

            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
        }
        
        public void InitializeSimulation(Player player, IRemotePlayerSimulation playerSimulation, SimulationObject simulation)
        {
            var prediction =
                new RemotePlayerMovementPrediction(_movementCommandPrediction, player.CharacterMovement);
            
            simulation.AddUpdatableSimulation(playerSimulation.PlayerMovePrediction.Initialize(prediction));
            _updatableContainer.QueryAdd(simulation);
            _updatableContainer.QueryAdd(player);

            simulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, simulation);
        }
    }
}