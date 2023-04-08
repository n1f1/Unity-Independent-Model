using Model;
using UnityEngine;

namespace Simulation.View.Factories
{
    public interface IViewFactory<out TView> where TView : IView
    {
        public TView Create(GameObject gameObject);
    }
}