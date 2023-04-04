using System;
using UI;

namespace GameMenu
{
    public class LaunchMultiplayer : ICommand
    {
        private IGameLoader _gameLoader;

        public LaunchMultiplayer(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Execute()
        {
        }
    }
}