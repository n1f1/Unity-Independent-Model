using System.Numerics;

namespace Model.SpatialObject
{
    public interface IPositionView
    {
        void UpdatePosition(Vector3 position);
    }
}