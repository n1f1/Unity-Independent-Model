using System.Numerics;
using Model.Shooting.Trajectory;

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