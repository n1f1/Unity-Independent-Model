using Model;
using UnityEngine;
using View;

namespace SceneViewSimulation
{
    internal class PlayerViewSimulationFactory : IViewSimulationFactory
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
}