using System;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedObjectPool<TObject>
{
    private readonly Dictionary<TObject, GameObject> _active;
    private readonly Stack<SimulablePair> _inactive;

    public SimulatedObjectPool(int capacity)
    {
        Capacity = capacity;
        _active = new Dictionary<TObject, GameObject>(capacity);
        _inactive = new Stack<SimulablePair>(capacity);
    }

    public int Capacity { get; }
    public bool CanGet() => _inactive.Count > 0;

    public void Return(TObject tObject)
    {
        if (_active.ContainsKey(tObject) == false)
            throw new ArgumentException();
        
        GameObject gameObject = _active[tObject];
        gameObject.SetActive(false);
        
        _inactive.Push(new SimulablePair
        {
            TObject = tObject, GameObject = gameObject
        });

        _active.Remove(tObject);
    }

    public SimulablePair Get()
    {
        SimulablePair simulablePair = _inactive.Pop();
        simulablePair.GameObject.SetActive(true);
        _active.Add(simulablePair.TObject, simulablePair.GameObject);
        
        return simulablePair;
    }

    public void Add(TObject tObject, GameObject gameObject)
    {
        _inactive.Push(new SimulablePair
        {
            TObject = tObject, GameObject = gameObject
        });
        
        gameObject.SetActive(false);
    }

    public class SimulablePair
    {
        public TObject TObject;
        public GameObject GameObject;
    }
}