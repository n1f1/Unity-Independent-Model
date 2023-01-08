using System.Numerics;

namespace Model.Characters.Shooting.Bullets
{
    public class NullTrajectory : ITrajectory
    {
        public Vector3 Evaluate(float ratio) =>
            Vector3.Zero;

        public float Distance => 0;
    }
}