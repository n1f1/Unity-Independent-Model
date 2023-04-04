using System;
using MultiPlayer;
using SinglePlayer;
using UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameMenu
{
    public class MainMenu
    {
        private readonly IGameLoader _gameLoader;

        public MainMenu(IGameLoader gameLoader)
        {
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Open()
        {
            MainMenuConfiguration configuration =
                Resources.Load<MainMenuConfiguration>(GameResourceConfigurations.MainMenuConfiguration);

            MainMenuScreen mainMenu = Object.Instantiate(configuration.MainMenuScreenTemplate);

            mainMenu.SinglePlayerButton.Construct(
                new CompositeCommand(
                    new CloseWindow(mainMenu.Window),
                    new LaunchSinglePlayer(_gameLoader)));

            mainMenu.MultiplayerButton.Construct(
                new CompositeCommand(
                    new CloseWindow(mainMenu.Window),
                    new LaunchMultiplayer(_gameLoader)));

            mainMenu.Window.Open();
        }
    }
}