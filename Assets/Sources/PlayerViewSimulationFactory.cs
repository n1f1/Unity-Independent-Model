using Model;
using UnityEngine;

internal class PlayerViewSimulationFactory : IViewSimulationFactory<Player>
{
    private readonly GameObjectFactory<Player> _gameObjectFactory;
    private GameObject _gameObject;

    public PlayerViewSimulationFactory(GameObjectFactory<Player> gameObjectFactory)
    {
        _gameObjectFactory = gameObjectFactory;
    }

    public IViewSimulation Create()
    {
        _gameObject = _gameObjectFactory.Instantiate();
        return new ViewSimulation(_gameObject);
    }
}