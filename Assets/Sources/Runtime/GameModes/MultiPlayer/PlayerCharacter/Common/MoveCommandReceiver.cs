using System;
using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Remote;
using Networking;
using Networking.PacketReceive;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public record MoveCommandReceiver : IReplicatedObjectReceiver<MoveCommand>
    {
        private readonly NotReconciledMovementCommands _notReconciledMovementCommands;
        private readonly PlayerClient _clientPlayer;
        private readonly IMovementCommandPrediction _movementCommandPrediction;

        public MoveCommandReceiver(NotReconciledMovementCommands reconciledMovement,
            PlayerClient clientPlayer, IMovementCommandPrediction movementCommandPrediction)
        {
            _movementCommandPrediction = movementCommandPrediction ??
                                       throw new ArgumentNullException(nameof(movementCommandPrediction));
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
            _notReconciledMovementCommands =
                reconciledMovement ?? throw new ArgumentNullException(nameof(reconciledMovement));
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
            _notReconciledMovementCommands.Reconcile(newCommand);
            IEnumerable<MoveCommand> notReconciled = _notReconciledMovementCommands.GetNotReconciled();
            
            newCommand.Execute();
            
            foreach (MoveCommand moveCommand in notReconciled) 
                moveCommand.Execute();
        }
    }
}