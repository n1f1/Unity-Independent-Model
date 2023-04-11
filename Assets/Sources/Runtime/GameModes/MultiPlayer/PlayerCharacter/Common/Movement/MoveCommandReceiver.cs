using System;
using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using Networking.PacketReceive;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Movement
{
    public record MoveCommandReceiver : IReplicatedObjectReceiver<MoveCommand>
    {
        private readonly NotReconciledCommands<MoveCommand> _notReconciledCommands;
        private readonly PlayerClient _clientPlayer;
        private readonly IMovementCommandPrediction _movementCommandPrediction;

        public MoveCommandReceiver(NotReconciledCommands<MoveCommand> reconciled,
            PlayerClient clientPlayer, IMovementCommandPrediction movementCommandPrediction)
        {
            _movementCommandPrediction = movementCommandPrediction ??
                                       throw new ArgumentNullException(nameof(movementCommandPrediction));
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
            _notReconciledCommands =
                reconciled ?? throw new ArgumentNullException(nameof(reconciled));
        }

        public void Receive(MoveCommand newCommand)
        {
            if (newCommand.Movement == _clientPlayer.Player.CharacterMovement)
                ProcessClientPlayerCommand(newCommand);
            else
                ProcessRemotePlayerCommand(newCommand);
        }

        private void ProcessRemotePlayerCommand(MoveCommand newCommand)
        {
            _movementCommandPrediction.PredictNextPacket(newCommand);
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