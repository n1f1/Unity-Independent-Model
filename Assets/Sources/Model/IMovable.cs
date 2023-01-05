using System.Numerics;

namespace Model
{
    public interface IMovable
    {
        void Move(float x, float z);
        void Move(Vector3 moveDelta);
    }
}