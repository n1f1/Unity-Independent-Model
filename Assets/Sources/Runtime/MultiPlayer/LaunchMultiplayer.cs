using System;
using GameMenu;
using UI;

namespace MultiPlayer
{
    public class LaunchMultiplayer : ICommand
    {
        private readonly IGameLoader _gameLoader;

        public LaunchMultiplayer(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Execute()
        {
            MultiplayerGame multiplayerGame = new MultiplayerGame(_gameLoader);
            _gameLoader.Load(multiplayerGame);
        }
    }
}