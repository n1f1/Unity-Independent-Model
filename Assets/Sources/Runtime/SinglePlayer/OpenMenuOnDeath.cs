using System;
using GameMenu;
using Model.Characters.CharacterHealth;

namespace SinglePlayer
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