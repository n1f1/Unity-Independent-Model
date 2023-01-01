using UnityEngine;
using Object = UnityEngine.Object;

namespace View
{
    public class GameObjectFactory
    {
        private readonly GameObject _template;

        public GameObjectFactory(GameObject template)
        {
            _template = template;
        }
        
        public GameObject Instantiate()
        {
            return Object.Instantiate(_template);
        }
    }
}