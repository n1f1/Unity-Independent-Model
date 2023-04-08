using Model.SpatialObject;
using UnityEngine;

namespace Simulation.View.Factories
{
    internal class PositionViewFactory : IViewFactory<IPositionView>
    {
        public IPositionView Create(GameObject gameObject) =>
            gameObject.AddComponent<PositionView>();
    }
}