using System.Numerics;
using Model.Shooting;
using NUnit.Framework;
using Tests.Model.Characters.Shooting.Support;

namespace Tests.Model.Characters.Shooting
{
    public class ForwardAimTests
    {
        [Test]
        public void StopsAiming()
        {
            ForwardAim forwardAim = new ForwardAim(new NullAimView());
            Assert.False(forwardAim.Aiming);
            forwardAim.Aim(Vector3.Zero, Vector3.Zero);
            Assert.True(forwardAim.Aiming);
            forwardAim.Stop();
            Assert.False(forwardAim.Aiming);
        }
    }
}