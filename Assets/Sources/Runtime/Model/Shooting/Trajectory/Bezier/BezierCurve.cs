using System;
using Vector3 = System.Numerics.Vector3;

namespace Model.Shooting.Trajectory.Bezier
{
    public static class BezierCurve
    {
        public static Vector3 GetOnBezier(in float t, in Vector3[] controlPoints)
        {
            int count = controlPoints.Length - 1;

            if (count > 16)
                throw new ArgumentOutOfRangeException(nameof(controlPoints.Length));

            if (t <= 0)
                return controlPoints[0];

            if (t >= 1)
                return controlPoints[^1];

            Vector3 p = new Vector3();

            for (int i = 0; i < controlPoints.Length; ++i)
                p += Bernstein(count, i, t) * controlPoints[i];

            return p;
        }

        private static float Bernstein(in int n, in int i, in float t)
        {
            float t_i = (float) Math.Pow(t, i);
            float t_n_minus_i = (float) Math.Pow(1 - t, n - i);

            float basis = Binomial(n, i) * t_i * t_n_minus_i;
            return basis;
        }

        private static float Binomial(in int n, in int i) =>
            Factorial[n] / (Factorial[i] * Factorial[n - i]);

        private static readonly float[] Factorial =
        {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
        };
    }
}