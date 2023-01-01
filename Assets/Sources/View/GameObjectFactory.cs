using UnityEngine;
using Object = UnityEngine.Object;

namespace View
{
    public class GameObjectFactory<T> : MonoBehaviour
    {
        [SerializeField] private GameObject _template;
    
        public GameObject Instantiate()
        {
            return Object.Instantiate(_template, transform.position, transform.rotation);
        }
    }
}