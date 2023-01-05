using Model.SpatialObject;
using UnityEngine;

namespace View.Factories
{
    internal class PositionViewFactory : IViewFactory<IPositionView>
    {
        public IPositionView Create(GameObject gameObject) =>
            gameObject.AddComponent<PositionView>();
    }
}