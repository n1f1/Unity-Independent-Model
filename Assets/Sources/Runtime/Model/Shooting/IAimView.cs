using System.Numerics;

namespace Model.Shooting
{
    public interface IAimView
    {
        void Aim(Vector3 position, Vector3 aimPosition);
        void Stop();
    }
}