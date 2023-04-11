using System.Numerics;

namespace Model.Shooting.Bullets
{
    public class NullTrajectory : ITrajectory
    {
        public Vector3 EvaluateForNormalizedRatio(float ratio) =>
            Vector3.Zero;

        public float Distance => 0;
        public Vector3 Start { get; }
        public Vector3 Finish { get; }
    }
}