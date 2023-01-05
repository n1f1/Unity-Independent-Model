using System.Collections.Generic;
using Model;

namespace Simulation.Common
{
    public class UpdatableContainer
    {
        private readonly LinkedList<IUpdatable> _simulations = new();
        private readonly Queue<IUpdatable> _removeQueue = new();
        private readonly Queue<IUpdatable> _addQueue = new();

        private bool _updating;

        public void Update(float deltaTime)
        {
            _updating = true;

            foreach (IUpdatable simulation in _simulations)
                simulation.UpdatePassedTime(deltaTime);

            _updating = false;

            for (int i = 0; i < _removeQueue.Count; i++)
                QueryRemove(_removeQueue.Dequeue());

            for (int i = 0; i < _addQueue.Count; i++)
                QueryAdd(_addQueue.Dequeue());
        }

        public void QueryAdd(IUpdatable updatable)
        {
            if (_updating == false)
                _simulations.AddLast(updatable);
            else
                _addQueue.Enqueue(updatable);
        }

        public void QueryRemove(IUpdatable updatable)
        {
            if (_updating == false)
                _simulations.Remove(updatable);
            else
                _removeQueue.Enqueue(updatable);
        }
    }
}