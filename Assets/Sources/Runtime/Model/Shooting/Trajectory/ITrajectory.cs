using System.Numerics;

namespace Model.Shooting.Trajectory
{
    public interface ITrajectory
    {
        Vector3 EvaluateForNormalizedRatio(float ratio);
        float Distance { get; }
        Vector3 Start { get; }
        Vector3 Finish { get; }
    }
}