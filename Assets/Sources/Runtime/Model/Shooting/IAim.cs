using System.Numerics;

namespace Model.Shooting
{
    public interface IAim
    {
        void Aim(Vector3 aimPosition, Vector3 from);
        ITrajectory Trajectory { get; }
        bool Aiming { get; }
        void Stop();
    }
}