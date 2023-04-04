using System;
using System.Threading.Tasks;
using GameMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : IGameLoader
{
    private readonly GameUpdate _gameUpdate;
    private AsyncOperation _sceneLoadingOperation;

    public GameLoader(GameUpdate gameUpdate)
    {
        _gameUpdate = gameUpdate ?? throw new ArgumentNullException(nameof(gameUpdate));
    }

    public async Task Load(IGame game)
    {
        _sceneLoadingOperation = SceneManager.LoadSceneAsync(1);

        while (!_sceneLoadingOperation.isDone)
            await Task.Yield();
        
        game.Load();
        _gameUpdate.AddUpdate(game);
    }
}