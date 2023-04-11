using System;
using UI.Commands;

namespace Menus.PauseMenu.Commands
{
    public class PauseGameCommand : ICommand
    {
        private readonly IPause _pause;

        public PauseGameCommand(IPause pause)
        {
            _pause = pause ?? throw new ArgumentNullException(nameof(pause));
        }

        public void Execute()
        {
            _pause.Pause();
        }
    }
}