using System;
using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Common;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
{
    public class NotReconciledMovementCommands
    {
        private readonly LinkedList<MoveCommand> _commands = new();
        private DateTime _lastCommandAddTime;

        public void Add(MoveCommand command) =>
            _commands.AddLast(command);

        public IEnumerable<MoveCommand> GetNotReconciled() =>
            _commands;

        public void Reconcile(MoveCommand createdObject)
        {
            for (LinkedListNode<MoveCommand> node = _commands.First; node != null; node = node.Next)
            {
                MoveCommand command = node.Value;

                if (command.ID <= createdObject.ID)
                    _commands.Remove(command);
            }
        }
    }
}