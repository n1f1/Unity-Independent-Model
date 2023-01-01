using System.Numerics;

namespace Model
{
    public class CompositePositionView : IPositionView
    {
        private readonly IPositionView[] _positionViews;

        public CompositePositionView(params IPositionView[] views)
        {
            _positionViews = views;
        }

        public void UpdatePosition(Vector3 position)
        {
            foreach (IPositionView positionView in _positionViews) 
                positionView.UpdatePosition(position);
        }
    }
}