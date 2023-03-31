using System;
using System.Collections.Generic;

namespace Simulation.Common
{
    /// <summary>
    /// Object Pool that stores pooled object with it's key.
    /// </summary>
    /// <typeparam name="TKeyObject"></typeparam>
    /// <typeparam name="TPoolable"></typeparam>
    public class KeyValueObjectPool<TKeyObject, TPoolable> where TPoolable : IPoolable
    {
        private readonly Dictionary<TKeyObject, TPoolable> _active;
        private readonly Stack<PooledPair> _inactive;

        public KeyValueObjectPool(int capacity = 4)
        {
            if (capacity <= 0)
                throw new ArgumentException();

            Capacity = capacity;
            _active = new Dictionary<TKeyObject, TPoolable>(capacity);
            _inactive = new Stack<PooledPair>(capacity);
        }

        public int Capacity { get; }
        public bool CanGet() => _inactive.Count > 0;

        public void Return(TKeyObject tObject)
        {
            if (tObject == null)
                throw new ArgumentNullException();

            if (_active.ContainsKey(tObject) == false)
                throw new InvalidOperationException();

            TPoolable poolable = _active[tObject];
            poolable.Disable();

            _inactive.Push(new PooledPair
            {
                TObject = tObject, Poolable = poolable
            });

            _active.Remove(tObject);
        }

        public PooledPair GetFree()
        {
            if (CanGet() == false)
                throw new InvalidOperationException();

            PooledPair pooledPair = _inactive.Pop();
            pooledPair.Poolable.Enable();
            _active.Add(pooledPair.TObject, pooledPair.Poolable);

            return pooledPair;
        }

        public void AddNewPair(TKeyObject tObject, TPoolable poolable)
        {
            if (tObject == null)
                throw new ArgumentException();

            if (poolable == null)
                throw new ArgumentException();

            _inactive.Push(new PooledPair
            {
                TObject = tObject, Poolable = poolable
            });

            poolable.Disable();
        }

        public void ReplaceKey(TKeyObject replaced, TKeyObject newObject)
        {
            if (_active.ContainsKey(replaced) == false)
                throw new ArgumentNullException();

            TPoolable poolable = _active[replaced];
            _active.Remove(replaced);
            _active.Add(newObject, poolable);
        }

        public class PooledPair
        {
            public TKeyObject TObject;
            public TPoolable Poolable;
        }
    }
}