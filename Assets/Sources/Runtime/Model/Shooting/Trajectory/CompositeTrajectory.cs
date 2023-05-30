using System;
using System.Linq;
using System.Numerics;

namespace Model.Shooting.Trajectory
{
    public class CompositeTrajectory : ITrajectory
    {
        private readonly ITrajectory[] _trajectories;

        public CompositeTrajectory(params ITrajectory[] trajectories)
        {
            _trajectories = trajectories ?? throw new ArgumentNullException(nameof(trajectories));
            Distance = _trajectories.Sum(trajectory => trajectory.Distance);
            Start = _trajectories[0].Start;
            Finish = _trajectories[^1].Finish;
        }

        public float Distance { get; }
        public Vector3 Start { get; }
        public Vector3 Finish { get; }

        public Vector3 EvaluateForNormalizedRatio(float ratio)
        {
            float distance = ratio * Distance;

            foreach (ITrajectory trajectory in _trajectories)
            {
                float trajectoryDistance = trajectory.Distance;
                
                if (distance <= trajectoryDistance)
                    return trajectory.EvaluateForNormalizedRatio(distance / trajectoryDistance);

                distance -= trajectoryDistance;
            }

            throw new InvalidOperationException();
        }
    }
}