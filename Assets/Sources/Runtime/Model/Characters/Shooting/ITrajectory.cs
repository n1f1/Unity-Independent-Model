using System.Numerics;

namespace Model.Characters.Shooting
{
    public interface ITrajectory
    {
        Vector3 EvaluateForNormalizedRatio(float ratio);
        float Distance { get; }
        Vector3 Start { get; }
        Vector3 Finish { get; }
    }
}