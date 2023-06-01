using System;
using GameModes.Game;
using GameModes.MultiPlayer;
using GameModes.SinglePlayer;
using UI.Commands;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Menus.MainMenu
{
    public class MainMenu : IGame
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

        public void Load()
        {
            Open();
        }

        public void UpdateTime(float deltaTime)
        {
        }
    }
}