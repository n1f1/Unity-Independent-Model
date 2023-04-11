using System;
using GameModes.Game;

namespace GameModes.MultiPlayer
{
    public class LaunchMultiplayer : UI.Commands.ICommand
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