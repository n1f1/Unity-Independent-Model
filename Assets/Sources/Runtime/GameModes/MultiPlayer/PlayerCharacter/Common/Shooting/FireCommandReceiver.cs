using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using Networking.PacketReceive;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Shooting
{
    public class FireCommandReceiver : IReplicatedObjectReceiver<FireCommand>
    {
        private readonly NotReconciledCommands<FireCommand> _notReconciledCommands;
        private readonly PlayerClient _clientPlayer;

        public FireCommandReceiver(NotReconciledCommands<FireCommand> notReconciledCommands, PlayerClient clientPlayer)
        {
            _notReconciledCommands =
                notReconciledCommands ?? throw new ArgumentNullException(nameof(notReconciledCommands));
            _clientPlayer = clientPlayer ?? throw new ArgumentNullException(nameof(clientPlayer));
        }

        public void Receive(FireCommand createdObject)
        {
            if (createdObject.Player == _clientPlayer.Player)
                ProcessClientCommand(createdObject);
            else
                ProcessRemoteCommand(createdObject);
        }

        private void ProcessRemoteCommand(FireCommand createdObject)
        {
            createdObject.Execute();
        }

        private void ProcessClientCommand(FireCommand command)
        {
            _notReconciledCommands.Reconcile(command);
        }
    }
}