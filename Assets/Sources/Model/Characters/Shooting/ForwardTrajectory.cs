using System.Numerics;

namespace Model.Characters.Shooting
{
    public class ForwardTrajectory : ITrajectory
    {
        private readonly Vector3 _start;
        private readonly Vector3 _finish;

        public ForwardTrajectory(Vector3 start, Vector3 finish)
        {
            _start = start;
            _finish = finish;
        }

        public float Distance => Vector3.Distance(_start, _finish);

        public Vector3 Evaluate(float ratio) => 
            Vector3.Lerp(_start, _finish, ratio);
    }
}