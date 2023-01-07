using System;
using System.Numerics;

namespace Model.SpatialObject
{
    public class Transform
    {
        private readonly IPositionView _positionView;

        public Transform(IPositionView positionView, Vector3 position = new())
        {
            _positionView = positionView ?? throw new ArgumentException();
            Position = position;
        }

        public Vector3 Position { get; private set; }

        public void SetPosition(float x, float z)
        {
            SetPosition(new Vector3(x, 0, z));
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
            _positionView.UpdatePosition(Position);
        }
    }
}