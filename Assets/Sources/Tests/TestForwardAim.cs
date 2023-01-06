using System.Numerics;
using Model.Characters.Shooting;
using NUnit.Framework;

namespace Tests
{
    public class TestForwardAim
    {
        [Test]
        public void Test_AimingUpdate()
        {
            ForwardAim forwardAim = new ForwardAim(new NullAimView());
            Assert.False(forwardAim.Aiming);
            forwardAim.Aim(Vector3.Zero, Vector3.Zero);
            Assert.True(forwardAim.Aiming);
            forwardAim.Stop();
            Assert.False(forwardAim.Aiming);
        }

        private class NullAimView : IAimView
        {
            public void Aim(Vector3 position, Vector3 aimPosition)
            {
            }

            public void Stop()
            {
            }
        }
    }
}