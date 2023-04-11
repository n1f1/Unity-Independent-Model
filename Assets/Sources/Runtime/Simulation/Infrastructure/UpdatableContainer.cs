using System;
using System.Collections.Generic;
using Model;

namespace Simulation.Infrastructure
{
    public class UpdatableContainer : IUpdatable
    {
        private readonly LinkedList<IUpdatable> _simulations = new();
        private readonly Queue<IUpdatable> _removeQueue = new();
        private readonly Queue<IUpdatable> _addQueue = new();

        private bool _updating;

        public void UpdateTime(float deltaTime)
        {
            if (deltaTime < 0)
                throw new ArgumentException();

            _updating = true;

            UpdateList(deltaTime);

            _updating = false;

            ProcessQueued();
        }

        public void QueryAdd(IUpdatable updatable)
        {
            if (updatable == null)
                throw new ArgumentException();

            if (_updating == false)
                _simulations.AddLast(updatable);
            else
                _addQueue.Enqueue(updatable);
        }

        public void QueryRemove(IUpdatable updatable)
        {
            if (updatable == null)
                throw new ArgumentException();

            if (_updating == false)
                _simulations.Remove(updatable);
            else
                _removeQueue.Enqueue(updatable);
        }

        private void UpdateList(float deltaTime)
        {
            foreach (IUpdatable simulation in _simulations)
                simulation.UpdateTime(deltaTime);
        }

        private void ProcessQueued()
        {
            for (int i = 0; i < _removeQueue.Count; i++)
                QueryRemove(_removeQueue.Dequeue());

            for (int i = 0; i < _addQueue.Count; i++)
                QueryAdd(_addQueue.Dequeue());
        }
    }
}