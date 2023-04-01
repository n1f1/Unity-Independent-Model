using System;
using System.Collections.Generic;

namespace Simulation.Common
{
    public class ObjectPool<TPoolable> : IObjectPool<TPoolable> where TPoolable : IPoolable
    {
        private readonly HashSet<TPoolable> _active;
        private readonly Stack<TPoolable> _inactive;

        public ObjectPool(int capacity = 4)
        {
            if (capacity <= 0)
                throw new ArgumentException();

            Capacity = capacity;
            _active = new HashSet<TPoolable>(capacity);
            _inactive = new Stack<TPoolable>(capacity);
        }

        public int Capacity { get; }
        public bool CanGet() => _inactive.Count > 0;

        public void Return(TPoolable poolable)
        {
            if (poolable == null)
                throw new ArgumentNullException();

            if (_active.Contains(poolable) == false)
                throw new InvalidOperationException();
            
            poolable.Disable();
            _inactive.Push(poolable);
            _active.Remove(poolable);
        }

        public TPoolable GetFree()
        {
            if (CanGet() == false)
                throw new InvalidOperationException();

            TPoolable pooled = _inactive.Pop();
            pooled.Enable();
            _active.Add(pooled);

            return pooled;
        }

        public void AddNew(TPoolable poolable)
        {
            if (poolable == null)
                throw new ArgumentException();

            if (poolable == null)
                throw new ArgumentException();

            _inactive.Push(poolable);
            poolable.Disable();
        }
    }
}