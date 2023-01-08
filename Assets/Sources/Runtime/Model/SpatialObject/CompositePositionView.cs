using System;
using System.Numerics;

namespace Model.SpatialObject
{
    public class CompositePositionView : IPositionView
    {
        private readonly IPositionView[] _positionViews;

        public CompositePositionView(params IPositionView[] views)
        {
            _positionViews = views ?? throw new ArgumentNullException();
        }

        public void UpdatePosition(Vector3 position)
        {
            foreach (IPositionView positionView in _positionViews)
                positionView.UpdatePosition(position);
        }
    }
}