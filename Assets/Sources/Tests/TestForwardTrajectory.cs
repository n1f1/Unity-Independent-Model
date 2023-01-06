using System;
using System.Numerics;
using Model.Characters.Shooting;
using NUnit.Framework;

namespace Tests
{
    public class TestForwardTrajectory
    {
        [Test]
        public void Test_DistanceProperty()
        {
            ForwardTrajectory trajectory = new ForwardTrajectory(Vector3.Zero, Vector3.UnitX);
            Assert.AreEqual(trajectory.Distance, 1);
        }
        
        [Test]
        public void Test_RatioEvaluateOutput()
        {
            ForwardTrajectory trajectory = new ForwardTrajectory(Vector3.Zero, Vector3.One);
            Assert.AreEqual(trajectory.Evaluate(0.5f), Vector3.One * 0.5f);
        }
        
        [Test]
        public void Test_EvaluateFail()
        {
            ForwardTrajectory trajectory = new ForwardTrajectory(Vector3.Zero, Vector3.One);
            Assert.Throws<ArgumentException>(() => trajectory.Evaluate(-1));
            Assert.Throws<ArgumentException>(() => trajectory.Evaluate(2));
        }
    }
}