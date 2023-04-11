using System;
using UI.Commands;

namespace Menus.PauseMenu.Commands
{
    public class UnpauseGameCommand : ICommand
    {
        private readonly IPause _pause;

        public UnpauseGameCommand(IPause pause)
        {
            _pause = pause ?? throw new ArgumentNullException(nameof(pause));
        }

        public void Execute()
        {
            _pause.Unpause();
        }
    }
}