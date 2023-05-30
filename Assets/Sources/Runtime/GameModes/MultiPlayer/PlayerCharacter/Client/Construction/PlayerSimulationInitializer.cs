using System;
using GameModes.MultiPlayer.PlayerCharacter.Client.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters;
using Model.Characters.Player;
using Networking.Common.PacketSend.ObjectSend;
using Simulation;
using Simulation.Characters.Player;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class ClientPlayerSimulationInitializer
    {
        private readonly IObjectToSimulationMap _objectToSimulation;
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private readonly INetworkObjectSender _sender;

        public ClientPlayerSimulationInitializer(IObjectToSimulationMap objectToSimulation,
            NotReconciledCommands<MoveCommand> reconciledMove,
            NotReconciledCommands<FireCommand> reconciledFire,
            INetworkObjectSender sender)
        {
            _objectToSimulation = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
            _notReconciledCommands = reconciledMove ?? throw new ArgumentNullException(nameof(reconciledMove));
            _notReconciledFireCommands = reconciledFire ?? throw new ArgumentNullException(nameof(reconciledFire));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public void InitializeSimulation(Player player, IPlayerSimulation playerSimulation, SimulationObject simulation,
            IPlayerView view, FakeHealthView fakeHealthView)
        {
            IMovable movable = new ClientPlayerMovementCommandSender(player, _sender, _notReconciledCommands);
            FireCommandSender fireCommandSender = new FireCommandSender(player, _sender, _notReconciledFireCommands);

            DamageableFakeView damageableFakeView =
                new DamageableFakeView(Player.MAXHealth, player.Health.Amount, fakeHealthView);
            
            simulation.AddSimulation(playerSimulation.Damageable.Initialize(damageableFakeView));
            player.Shooter.Exclude(damageableFakeView);

            simulation.AddUpdatableSimulation(playerSimulation.Movable.Initialize(movable));
            simulation.AddUpdatableSimulation(playerSimulation.CharacterShooter.Initialize(fireCommandSender));

            simulation.Enable();
            _objectToSimulation.RegisterNew(player, simulation);
        }
    }
}