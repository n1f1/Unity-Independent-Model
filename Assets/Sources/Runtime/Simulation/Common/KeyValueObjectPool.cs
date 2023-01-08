using System;
using System.Collections.Generic;

namespace Simulation.Common
{
    public class KeyValueObjectPool<TObject, TPoolable> where TPoolable : IPoolable
    {
        private readonly Dictionary<TObject, TPoolable> _active;
        private readonly Stack<SimulatedPair> _inactive;

        public KeyValueObjectPool(int capacity = 4)
        {
            if (capacity <= 0)
                throw new ArgumentException();

            Capacity = capacity;
            _active = new Dictionary<TObject, TPoolable>(capacity);
            _inactive = new Stack<SimulatedPair>(capacity);
        }

        public int Capacity { get; }
        public bool CanGet() => _inactive.Count > 0;

        public void Return(TObject tObject)
        {
            if (tObject == null)
                throw new ArgumentNullException();

            if (_active.ContainsKey(tObject) == false)
                throw new InvalidOperationException();

            TPoolable poolable = _active[tObject];
            poolable.Disable();

            _inactive.Push(new SimulatedPair
            {
                TObject = tObject, Poolable = poolable
            });

            _active.Remove(tObject);
        }

        public SimulatedPair Get()
        {
            if (CanGet() == false)
                throw new InvalidOperationException();

            SimulatedPair simulatedPair = _inactive.Pop();
            simulatedPair.Poolable.Enable();
            _active.Add(simulatedPair.TObject, simulatedPair.Poolable);

            return simulatedPair;
        }

        public void AddNew(TObject tObject, TPoolable poolable)
        {
            if (tObject == null)
                throw new ArgumentException();

            if (poolable == null)
                throw new ArgumentException();

            _inactive.Push(new SimulatedPair
            {
                TObject = tObject, Poolable = poolable
            });

            poolable.Disable();
        }

        public void Replace(TObject replaced, TObject newObject)
        {
            if (_active.ContainsKey(replaced) == false)
                throw new ArgumentNullException();

            TPoolable poolable = _active[replaced];
            _active.Remove(replaced);
            _active.Add(newObject, poolable);
        }

        public class SimulatedPair
        {
            public TObject TObject;
            public TPoolable Poolable;
        }
    }
}