using System;
using SinglePlayer;
using UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameMenu
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
                    new HideObject(menuScreen.PauseButton.gameObject),
                    new OpenWindow(menuScreen.Window),
                    new PauseGame(_pause)));

            menuScreen.ResumeGame.Construct(
                new CompositeCommand(
                    new DisplayObject(menuScreen.PauseButton.gameObject),
                    new CloseWindow(menuScreen.Window),
                    new UnpauseGame(_pause)));
        }
    }
}