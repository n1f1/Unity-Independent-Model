using System;
using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using Model.Characters.Player;
using Simulation;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    public class RemotePlayerSimulationInitializer
    {
        private readonly IObjectToSimulationMap _objectToSimulationMapping;
        private readonly IMovementCommandPrediction _movementCommandPrediction;
        private readonly UpdatableContainer _updatableContainer;

        public RemotePlayerSimulationInitializer(IObjectToSimulationMap objectToSimulationMapping,
            UpdatableContainer updatableContainer,
            IMovementCommandPrediction movementCommandPrediction)
        {
            _movementCommandPrediction = movementCommandPrediction;
            _updatableContainer = updatableContainer;

            _objectToSimulationMapping =
                objectToSimulationMapping ?? throw new ArgumentNullException(nameof(objectToSimulationMapping));
        }

        public void InitializeSimulation(Player player, IRemotePlayerSimulation playerSimulation,
            SimulationObject simulation, IPlayerView view)
        {
            var prediction =
                new RemotePlayerMovementPrediction(_movementCommandPrediction, player.CharacterMovement);

            DamageableFakeView damageableFakeView =
                new DamageableFakeView(Player.MAXHealth, player.Health.Amount, view.HealthView);
            
            simulation.AddSimulation(playerSimulation.Damageable.Initialize(damageableFakeView));
            player.Shooter.Exclude(damageableFakeView);

            simulation.AddUpdatableSimulation(playerSimulation.PlayerMovePrediction.Initialize(prediction));
            _updatableContainer.QueryAdd(simulation);
            _updatableContainer.QueryAdd(player);

            simulation.Enable();
            _objectToSimulationMapping.RegisterNew(player, simulation);
        }
    }
}