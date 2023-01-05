using System.Numerics;

namespace Model
{
    public interface IAimView : IView
    {
        void Aim(Vector3 position, Vector3 aimPosition);
        void Stop();
    }
}