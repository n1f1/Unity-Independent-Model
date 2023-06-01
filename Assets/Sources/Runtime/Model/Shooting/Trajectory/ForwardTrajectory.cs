﻿using System;
using System.Numerics;

namespace Model.Shooting.Trajectory
{
    public class ForwardTrajectory : ITrajectory
    {
        public ForwardTrajectory(Vector3 start, Vector3 finish)
        {
            Start = start;
            Finish = finish;
            Distance = Vector3.Distance(Start, Finish);
        }

        public Vector3 Start { get; }
        public Vector3 Finish { get; }
        public float Distance { get; }

        public Vector3 EvaluateForNormalizedRatio(float ratio)
        {
            if (ratio is < 0 or > 1)
                throw new ArgumentOutOfRangeException();

            return Vector3.Lerp(Start, Finish, ratio);
        }
    }
}