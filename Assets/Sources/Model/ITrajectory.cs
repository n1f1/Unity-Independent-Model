using System.Numerics;

namespace Model
{
    public interface ITrajectory
    {
        Vector3 Evaluate(float ratio);
        float Distance { get; }
    }
}