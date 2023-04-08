using System.Numerics;

namespace Model.Characters
{
    public interface IMovable
    {
        void Move(Vector3 direction, float deltaTime);
    }
}