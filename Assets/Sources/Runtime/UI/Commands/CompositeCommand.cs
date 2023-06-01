using System;

namespace UI.Commands
{
    public class CompositeCommand : ICommand
    {
        private readonly ICommand[] _commands;

        public CompositeCommand(params ICommand[] commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public void Execute()
        {
            foreach (ICommand command in _commands)
                command.Execute();
        }
    }
}