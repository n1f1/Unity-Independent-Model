using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Common
{
    public class SimulatedSimulationPool<TKey, TPooled> where TPooled : IPoolable
    {
        private readonly Dictionary<TKey, TPooled> _keyToActiveInPool = new();
        private readonly Dictionary<TKey, TPooled> _keysOfInactiveInPool = new();

        public SimulatedSimulationPool(int capacity = 4)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }

        public bool CanGet() => _keysOfInactiveInPool.Count > 0;

        public TKey GetFreeByKey()
        {
            TKey key = _keysOfInactiveInPool.FirstOrDefault().Key;

            if (key == null)
                throw new InvalidOperationException();

            TPooled pooled = _keysOfInactiveInPool[key];
            pooled.Enable();
            _keysOfInactiveInPool.Remove(key);
            _keyToActiveInPool.Add(key, pooled);

            return key;
        }

        public void AddNew(TKey key, TPooled poolable)
        {
            _keysOfInactiveInPool.Add(key, poolable);
            poolable.Disable();
        }

        public void Return(TKey key)
        {
            TPooled pooled = _keyToActiveInPool[key];
            pooled.Disable();
            _keyToActiveInPool.Remove(key);
            _keysOfInactiveInPool.Add(key, pooled);
        }

        public TPooled Remove(TKey key)
        {
            TPooled pooled = _keyToActiveInPool[key];
            pooled.Disable();
            _keyToActiveInPool.Remove(key);
            
            return pooled;
        }
    }
}