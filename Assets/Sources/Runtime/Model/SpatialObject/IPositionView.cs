using System.Numerics;

namespace Model.SpatialObject
{
    public interface IPositionView : IView
    {
        void UpdatePosition(Vector3 position);
    }
}