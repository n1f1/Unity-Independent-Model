using System.Collections.Generic;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation
{
    public class NotReconciledCommands<TCommand> where TCommand : IReconciliationCommand
    {
        private readonly LinkedList<TCommand> _commands = new();

        public void Add(TCommand command) =>
            _commands.AddLast(command);

        public IEnumerable<TCommand> GetNotReconciled() =>
            _commands;

        public void ReconcileAllBefore(TCommand newCommand)
        {
            for (LinkedListNode<TCommand> node = _commands.First; node != null; node = node.Next)
            {
                TCommand command = node.Value;

                if (command.ID <= newCommand.ID)
                    _commands.Remove(command);
            }
        }

        public void Reconcile(TCommand newCommand)
        {
            for (LinkedListNode<TCommand> node = _commands.First; node != null; node = node.Next)
            {
                TCommand command = node.Value;

                if (command.ID == newCommand.ID)
                    _commands.Remove(command);
            }
        }
    }
}