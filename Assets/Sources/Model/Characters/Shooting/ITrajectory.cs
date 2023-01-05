using System.Numerics;

namespace Model.Characters.Shooting
{
    public interface ITrajectory
    {
        Vector3 Evaluate(float ratio);
        float Distance { get; }
    }
}