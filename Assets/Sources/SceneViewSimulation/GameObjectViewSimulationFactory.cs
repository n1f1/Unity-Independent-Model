using Model;
using UnityEngine;
using View;

namespace SceneViewSimulation
{
    internal class GameObjectViewSimulationFactory : IViewSimulationFactory
    {
        private readonly GameObjectFactory _gameObjectFactory;
        private GameObject _gameObject;

        public GameObjectViewSimulationFactory(GameObjectFactory gameObjectFactory)
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