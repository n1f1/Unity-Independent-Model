using System;
using GameModes.Game;
using UI.Commands;

namespace Menus.MainMenu
{
    public class OpenMainMenu : ICommand
    {
        private readonly IGameLoader _gameLoader;

        public OpenMainMenu(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Execute()
        {
            _gameLoader.Load(new MainMenu(_gameLoader));
        }
    }
}