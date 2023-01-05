using System.Numerics;

namespace Model
{
    public class ForwardTrajectory : ITrajectory
    {
        private Vector3 _start;
        private Vector3 _finish;

        public ForwardTrajectory(Vector3 start, Vector3 finish)
        {
            _start = start;
            _finish = finish;
        }

        public float Distance => Vector3.Distance(_start, _finish);

        public Vector3 Evaluate(float ratio)
        {
            return Vector3.Lerp(_start, _finish, ratio);
        }
    }
}