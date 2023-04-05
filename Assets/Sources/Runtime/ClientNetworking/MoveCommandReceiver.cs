using System;
using System.Collections.Generic;
using MultiPlayer;
using Networking;

namespace ClientNetworking
{
    public record MoveCommandReceiver : IReplicatedObjectReceiver<MoveCommand>
    {
        private readonly NotReconciledMovementCommands _notReconciledMovementCommands;
        private readonly PlayerClient _clientPlayer;

        public MoveCommandReceiver(NotReconciledMovementCommands reconciledMovement,
            PlayerClient clientPlayer)
        {
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
            _notReconciledMovementCommands =
                reconciledMovement ?? throw new ArgumentNullException(nameof(reconciledMovement));
        }

        public void Receive(MoveCommand newCommand)
        {
            if (_clientPlayer.Player.CharacterMovement == newCommand.Movement)
                ProcessClientPlayerCommand(newCommand);
            else
                ProcessNonClientPlayerCommand(newCommand);
        }

        private void ProcessNonClientPlayerCommand(MoveCommand newCommand)
        {
            newCommand.Execute();
        }

        private void ProcessClientPlayerCommand(MoveCommand newCommand)
        {
            _notReconciledMovementCommands.Reconcile(newCommand);
            IEnumerable<MoveCommand> toExecute = _notReconciledMovementCommands.GetNotReconciled();

            newCommand.Execute();

            foreach (MoveCommand moveCommand in toExecute)
                moveCommand.Execute();
        }
    }
}