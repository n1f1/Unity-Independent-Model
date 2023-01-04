using System.Numerics;

namespace Model
{
    public interface IPositionView : IView
    {
        void UpdatePosition(Vector3 position);
    }
}