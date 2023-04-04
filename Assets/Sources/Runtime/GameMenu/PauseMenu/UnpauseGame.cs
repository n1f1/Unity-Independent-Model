using System;
using SinglePlayer;
using UI;

namespace GameMenu
{
    public class UnpauseGame : ICommand
    {
        private readonly IPause _pause;

        public UnpauseGame(IPause pause)
        {
            _pause = pause ?? throw new ArgumentNullException(nameof(pause));
        }

        public void Execute()
        {
            _pause.Unpause();
        }
    }
}