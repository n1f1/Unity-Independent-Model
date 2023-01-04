using Model;
using UnityEngine;

namespace View
{
    internal class PositionViewFactory : IViewFactory<IPositionView>
    {
        public IPositionView Create(GameObject gameObject) => 
            gameObject.AddComponent<PositionView>();
    }
}