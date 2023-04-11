using System;
using GameModes;
using GameModes.Game;
using GameModes.SinglePlayer;
using Menus.MainMenu;
using Menus.PauseMenu.Commands;
using UI.Commands;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Menus.PauseMenu
{
    public class PauseMenu
    {
        private readonly IGameLoader _gameLoader;
        private readonly IPause _pause;

        public PauseMenu(IGameLoader gameLoader, IPause gameStatus)
        {
            _pause = gameStatus ?? throw new ArgumentNullException(nameof(gameStatus));
            _gameLoader = gameLoader ?? throw new ArgumentNullException(nameof(gameLoader));
        }

        public void Create()
        {
            PauseMenuConfiguration configuration =
                Resources.Load<PauseMenuConfiguration>(GameResourceConfigurations.PauseMenuConfiguration);

            PauseMenuScreen menuScreen = Object.Instantiate(configuration.PauseScreenTemplate);

            menuScreen.MainMenu.Construct(new OpenMainMenu(_gameLoader));

            menuScreen.PauseButton.Construct(
                new CompositeCommand(
                    new HideObjectCommand(menuScreen.PauseButton.gameObject),
                    new OpenWindow(menuScreen.Window),
                    new PauseGameCommand(_pause)));

            menuScreen.ResumeGame.Construct(
                new CompositeCommand(
                    new DisplayObjectCommand(menuScreen.PauseButton.gameObject),
                    new CloseWindow(menuScreen.Window),
                    new UnpauseGameCommand(_pause)));
        }
    }
}