using System;
using GameModes;
using UI;

namespace GameMenu
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
            _gameLoader.Load(new NullGame());
            MainMenu mainMenu = new MainMenu(_gameLoader);
            mainMenu.Open();
        }
    }
}