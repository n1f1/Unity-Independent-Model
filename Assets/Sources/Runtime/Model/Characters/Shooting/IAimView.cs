using System.Numerics;

namespace Model.Characters.Shooting
{
    public interface IAimView : IView
    {
        void Aim(Vector3 position, Vector3 aimPosition);
        void Stop();
    }
}