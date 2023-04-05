using System.Collections.Generic;

namespace ClientNetworking
{
    public class NotReconciledMovementCommands
    {
        private readonly LinkedList<MoveCommand> _commands = new();

        public void Add(MoveCommand command)
        {
            _commands.AddLast(command);
        }

        public void Reconcile(MoveCommand createdObject)
        {
            for (LinkedListNode<MoveCommand> node = _commands.First; node != null; node = node.Next)
            {
                MoveCommand command = node.Value;

                if (command.CreationTime <= createdObject.CreationTime)
                    _commands.Remove(command);
            }
        }

        public IEnumerable<MoveCommand> GetNotReconciled() => 
            _commands;
    }
}