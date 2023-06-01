using System;
using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using Networking.Common.PacketReceive;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Movement
{
    public record MoveCommandReceiver : IReplicatedObjectReceiver<MoveCommand>
    {
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly ClientPlayerSimulation _simulationClientPlayer;
        private readonly IMovementCommandPrediction _movementPrediction;

        public MoveCommandReceiver(NotReconciledCommands<MoveCommand> reconciled,
            ClientPlayerSimulation simulation, IMovementCommandPrediction movementPrediction)
        {
            _movementPrediction = movementPrediction ?? throw new ArgumentNullException(nameof(movementPrediction));
            _simulationClientPlayer = simulation ?? throw new ArgumentNullException(nameof(simulation));
            _notReconciledCommands = reconciled ?? throw new ArgumentNullException(nameof(reconciled));
        }

        public void Receive(MoveCommand newCommand)
        {
            if (newCommand.Movement == _simulationClientPlayer.Player.CharacterMovement)
                ProcessClientPlayerCommand(newCommand);
            else
                ProcessRemotePlayerCommand(newCommand);
        }

        private void ProcessRemotePlayerCommand(MoveCommand newCommand)
        {
            _movementPrediction.PredictNextPacket(newCommand);
        }

        private void ProcessClientPlayerCommand(MoveCommand newCommand)
        {
            _notReconciledCommands.ReconcileAllBefore(newCommand);
            IEnumerable<MoveCommand> notReconciled = _notReconciledCommands.GetNotReconciled();

            newCommand.Execute();

            foreach (MoveCommand moveCommand in notReconciled)
                moveCommand.Execute();
        }
    }
}