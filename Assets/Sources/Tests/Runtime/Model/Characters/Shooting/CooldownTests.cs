using System;
using Model.Shooting;
using NUnit.Framework;

namespace Tests.Model.Characters.Shooting
{
    public class CooldownTest
    {
        [Test]
        public void CanNotInitializeWithInvalidValue()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cooldown(-1f));
        }
        
        [Test]
        public void CooldownElapsesInRightTime()
        {
            Cooldown cooldown = new Cooldown(1f);
            Assert.IsFalse(cooldown.IsReady);
            cooldown.ReduceTime(1f);
            Assert.IsTrue(cooldown.IsReady);
        }
        
        [Test]
        public void ResetsToValidState()
        {
            Cooldown cooldown = new Cooldown(1f);
            cooldown.ReduceTime(1f);
            cooldown.Reset();
            Assert.IsFalse(cooldown.IsReady);
            cooldown.ReduceTime((float) (1f - 1E-5));
            Assert.IsFalse(cooldown.IsReady);
        }
        
        [Test]
        public void ReducesOnlyValidRanges()
        {
            Cooldown cooldown = new Cooldown(1f);
            Assert.Throws<ArgumentOutOfRangeException>(() => cooldown.ReduceTime(-1f));
            cooldown.ReduceTime(0.5f);
            Assert.IsFalse(cooldown.IsReady);
            cooldown.ReduceTime(0.5f);
            Assert.IsTrue(cooldown.IsReady);
        }
    }
}