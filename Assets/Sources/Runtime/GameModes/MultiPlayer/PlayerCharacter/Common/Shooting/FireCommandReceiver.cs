using System;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using Networking.Common.PacketReceive;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Shooting
{
    public class FireCommandReceiver : IReplicatedObjectReceiver<FireCommand>
    {
        private readonly NotReconciledCommands<FireCommand> _notReconciledCommands;
        private readonly ClientPlayerSimulation _simulationClientPlayer;

        public FireCommandReceiver(NotReconciledCommands<FireCommand> notReconciledCommands,
            ClientPlayerSimulation simulationClientPlayer)
        {
            _notReconciledCommands =
                notReconciledCommands ?? throw new ArgumentNullException(nameof(notReconciledCommands));
            _simulationClientPlayer =
                simulationClientPlayer ?? throw new ArgumentNullException(nameof(simulationClientPlayer));
        }

        public void Receive(FireCommand createdObject)
        {
            if (createdObject.Player == _simulationClientPlayer.Player)
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