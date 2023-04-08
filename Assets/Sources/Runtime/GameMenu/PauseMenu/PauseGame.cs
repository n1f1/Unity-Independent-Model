using System;
using GameModes.SinglePlayer;
using UI;

namespace GameMenu.PauseMenu
{
    public class PauseGame : ICommand
    {
        private readonly IPause _pause;

        public PauseGame(IPause pause)
        {
            _pause = pause ?? throw new ArgumentNullException(nameof(pause));
        }

        public void Execute()
        {
            _pause.Pause();
        }
    }
}