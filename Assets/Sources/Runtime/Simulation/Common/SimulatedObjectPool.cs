using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Common
{
    public class SimulatedObjectPool<TObject>
    {
        private readonly Dictionary<TObject, GameObject> _active;
        private readonly Stack<SimulatedPair> _inactive;

        public SimulatedObjectPool(int capacity = 4)
        {
            if (capacity <= 0)
                throw new ArgumentException();
            
            Capacity = capacity;
            _active = new Dictionary<TObject, GameObject>(capacity);
            _inactive = new Stack<SimulatedPair>(capacity);
        }

        public int Capacity { get; }
        public bool CanGet() => _inactive.Count > 0;

        public void Return(TObject tObject)
        {
            if (_active.ContainsKey(tObject) == false)
                throw new ArgumentException();

            GameObject gameObject = _active[tObject];
            gameObject.SetActive(false);

            _inactive.Push(new SimulatedPair
            {
                TObject = tObject, GameObject = gameObject
            });

            _active.Remove(tObject);
        }

        public SimulatedPair Get()
        {
            SimulatedPair simulatedPair = _inactive.Pop();
            simulatedPair.GameObject.SetActive(true);
            _active.Add(simulatedPair.TObject, simulatedPair.GameObject);

            return simulatedPair;
        }

        public void AddNew(TObject tObject, GameObject gameObject)
        {
            if (tObject == null)
                throw new ArgumentException();

            if (gameObject == null)
                throw new ArgumentException();
            
            _inactive.Push(new SimulatedPair
            {
                TObject = tObject, GameObject = gameObject
            });

            gameObject.SetActive(false);
        }

        public class SimulatedPair
        {
            public TObject TObject;
            public GameObject GameObject;
        }
    }
}