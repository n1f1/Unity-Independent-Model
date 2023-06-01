using System.Numerics;

namespace Model.Shooting.Trajectory.Bezier
{
    public class BezierTrajectory : ITrajectory
    {
        private readonly Vector3[] _controlPoints;

        public BezierTrajectory(Vector3 start, Vector3 curve, Vector3 finish, float distance)
        {
            Start = start;
            Finish = finish;
            Distance = distance;
            _controlPoints = new[] {Start, curve, Finish};
        }

        public Vector3 Start { get; }
        public Vector3 Finish { get; }
        public float Distance { get; }

        public Vector3 EvaluateForNormalizedRatio(float ratio) =>
            BezierCurve.GetOnBezier(ratio, _controlPoints);
    }
}