using System;
using System.Collections.Generic;
using Model;

namespace Simulation
{
    public class SimulationObject<TTemplate> : IUpdatable
    {
        private readonly Dictionary<Type, object> _simulations = new();
        private readonly Dictionary<Type, IView> _views = new();
        private readonly List<IUpdatable> _updatableList = new();

        public SimulationObject(TTemplate template)
        {
            Template = template ?? throw new ArgumentNullException();
        }

        public TTemplate Template { get; }

        public void Add<TView>(TView view) where TView : IView
        {
            _views.Add(typeof(TView), view);
        }

        public TView GetView<TView>() where TView : IView => 
            (TView) _views[typeof(TView)];

        public void AddUpdatableSimulation<TSimulated>(ISimulation<TSimulated> simulation)
        {
            if (_updatableList.Contains(simulation) || _simulations.ContainsKey(typeof(TSimulated)))
                throw new InvalidOperationException();
            
            AddSimulation(simulation);
            _updatableList.Add(simulation);
        }

        public void AddSimulation<TSimulated>(ISimulation<TSimulated> simulation) => 
            _simulations.Add(typeof(TSimulated), simulation);

        public ISimulation<T1> GetSimulation<T1>() => 
            _simulations[typeof(T1)] as ISimulation<T1>;

        public void UpdateTime(float deltaTime)
        {
            foreach (IUpdatable updatable in _updatableList) 
                updatable.UpdateTime(deltaTime);
        }

        public void Clear()
        {
            _simulations.Clear();
            _updatableList.Clear();
        }
    }
}