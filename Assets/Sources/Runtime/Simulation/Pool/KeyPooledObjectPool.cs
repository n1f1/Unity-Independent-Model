using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.Pool
{
    /// <summary>
    /// Object pool that binds pooled object to key-object and enables default pooling operations via key-object.
    /// </summary>
    /// <typeparam name="TKey">key bound to pooled object</typeparam>
    /// <typeparam name="TPooled">pooled object</typeparam>
    public class KeyPooledObjectPool<TKey, TPooled> where TPooled : IPoolable
    {
        private readonly Dictionary<TKey, TPooled> _keyToActiveInPool = new();
        private readonly Dictionary<TKey, TPooled> _keysOfInactiveInPool = new();

        public KeyPooledObjectPool(int capacity = 4)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }

        public bool CanGet() => _keysOfInactiveInPool.Count > 0;

        public TKey GetFreeByKey()
        {
            if (_keysOfInactiveInPool.Count == 0)
                throw new InvalidOperationException();

            TKey key = _keysOfInactiveInPool.FirstOrDefault().Key;
            TPooled pooled = _keysOfInactiveInPool[key];
            pooled.Enable();
            _keysOfInactiveInPool.Remove(key);
            _keyToActiveInPool.Add(key, pooled);

            return key;
        }

        public void AddNew(TKey key, TPooled poolable)
        {
            if (_keysOfInactiveInPool.ContainsKey(key))
                throw new InvalidOperationException();

            if (_keyToActiveInPool.ContainsKey(key))
                throw new InvalidOperationException();

            _keysOfInactiveInPool.Add(key, poolable);
            poolable.Disable();
        }

        public void ReturnInactive(TKey key)
        {
            TPooled pooled = _keyToActiveInPool[key];
            pooled.Disable();
            _keyToActiveInPool.Remove(key);
            _keysOfInactiveInPool.Add(key, pooled);
        }

        public TPooled RemoveActive(TKey key)
        {
            TPooled pooled = _keyToActiveInPool[key];
            pooled.Disable();
            _keyToActiveInPool.Remove(key);

            return pooled;
        }
    }
}