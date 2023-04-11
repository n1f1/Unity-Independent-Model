using System;
using GameModes.Game;
using UI;
using UI.Commands;

namespace GameModes.SinglePlayer
{
    public class LaunchSinglePlayer : ICommand
    {
        private readonly IGameLoader _gameLoader;

        public LaunchSinglePlayer(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Execute()
        {
            SinglePlayerGame game = new SinglePlayerGame(_gameLoader);
            _gameLoader.Load(game);
        }
    }
}