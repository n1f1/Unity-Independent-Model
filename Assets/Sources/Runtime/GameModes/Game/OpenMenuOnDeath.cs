using System;
using Menus.MainMenu;
using Model.Characters.CharacterHealth;

namespace GameModes.Game
{
    public class OpenMenuOnDeath : IDeathView
    {
        private readonly IGameLoader _gameLoader;

        public OpenMenuOnDeath(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Die()
        {
            MainMenu mainMenu = new MainMenu(_gameLoader);
            mainMenu.Open();
        }
    }
}