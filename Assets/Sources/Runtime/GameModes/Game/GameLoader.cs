using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameModes.Game
{
    public class GameLoader : IGameLoader
    {
        private readonly GameUpdate _gameUpdate;
        private AsyncOperation _sceneLoadingOperation;
        private IGame _game;

        public GameLoader(GameUpdate gameUpdate)
        {
            _gameUpdate = gameUpdate ?? throw new ArgumentNullException(nameof(gameUpdate));
        }

        public async void Load(IGame game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            _gameUpdate.Remove(_game);
            _game = game;

            _sceneLoadingOperation = SceneManager.LoadSceneAsync(1);

            while (!_sceneLoadingOperation.isDone)
                await Task.Yield();

            game.Load();
            _gameUpdate.AddUpdate(game);
        }
    }
}