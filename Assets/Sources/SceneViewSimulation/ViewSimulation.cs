using System;
using Model;
using Simulation;
using UnityEngine;
using View;

namespace SceneViewSimulation
{
    internal class ViewSimulation : IViewSimulation
    {
        private readonly GameObject _gameObject;

        public ViewSimulation(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public T AddView<T>() where T : IView
        {
            T view;
        
            if (typeof(T) == typeof(IPositionView))
                view = (T) (IView) _gameObject.AddComponent<PositionView>();
            else
                throw new InvalidOperationException();

            return view;
        }

        public ISimulation AddSimulation<TSimulation>(TSimulation simulation)
        {
            if (typeof(TSimulation) == typeof(CharacterMovement))
                return _gameObject.AddComponent<PlayerMovement>().Initialize(simulation as CharacterMovement, new AxisInput());
            else
                throw new InvalidOperationException();
        }
    }
}