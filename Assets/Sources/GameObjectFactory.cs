using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameObjectFactory<T> : MonoBehaviour
{
    [SerializeField] private GameObject _template;
    
    private Action _action;

    public GameObject Instantiate()
    {
        return Object.Instantiate(_template, transform.position, transform.rotation);
    }

    public void Add(Action action)
    {
        _action = action;
    }

    private void Update()
    {
        _action?.Invoke();
    }
}