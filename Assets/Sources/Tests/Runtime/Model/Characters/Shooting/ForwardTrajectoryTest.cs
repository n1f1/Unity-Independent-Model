using System;
using System.Numerics;
using Model.Shooting;
using NUnit.Framework;

namespace Tests.Model.Characters.Shooting
{
    public class TestForwardTrajectory
    {
        [Test]
        public void CanNotEvaluateNotNormalizedRatio()
        {
            ForwardTrajectory trajectory = new ForwardTrajectory(Vector3.Zero, Vector3.One);
            Assert.Throws<ArgumentOutOfRangeException>(() => trajectory.EvaluateForNormalizedRatio(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => trajectory.EvaluateForNormalizedRatio(2));
        }

        [Test]
        public void EvaluatesRightValue()
        {
            ForwardTrajectory trajectory = new ForwardTrajectory(Vector3.Zero, Vector3.One);
            Assert.AreEqual(trajectory.EvaluateForNormalizedRatio(0.5f), Vector3.One * 0.5f);
        }

        [Test]
        public void ReturnsRightDistance()
        {
            ForwardTrajectory trajectory = new ForwardTrajectory(Vector3.Zero, Vector3.UnitX);
            Assert.AreEqual(trajectory.Distance, 1);
        }
    }
}