using System.Numerics;

namespace Model
{
    public class Transform
    {
        private IPositionView _positionView;

        public Transform(IPositionView positionView)
        {
            _positionView = positionView;
        }

        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public void SetPosition(float x, float z)
        {
            Position = new Vector3(x, 0, z);
            _positionView.UpdatePosition(Position);
        }
    }
}