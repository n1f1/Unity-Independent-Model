using System;
using Model.Characters.Shooting;
using NUnit.Framework;

namespace Tests
{
    public class TestCooldown
    {
        [Test]
        public void Test_Construction()
        {
            Assert.DoesNotThrow(() => new Cooldown(1f));
            Assert.Throws<ArgumentException>(() => new Cooldown(-1f));
        }
        
        [Test]
        public void Test_IsReady()
        {
            Cooldown cooldown = new Cooldown(1f);
            Assert.IsFalse(cooldown.IsReady);
            cooldown.ReduceTime(1f);
            Assert.IsTrue(cooldown.IsReady);
        }
        
        [Test]
        public void Test_Reset()
        {
            Cooldown cooldown = new Cooldown(1f);
            cooldown.ReduceTime(1f);
            cooldown.Reset();
            Assert.IsFalse(cooldown.IsReady);
            cooldown.ReduceTime((float) (1f - 1E-5));
            Assert.IsFalse(cooldown.IsReady);
        }
        
        [Test]
        public void Test_ReduceTimeValid()
        {
            Cooldown cooldown = new Cooldown(1f);
            Assert.Throws<ArgumentException>(() => cooldown.ReduceTime(-1f));
            cooldown.ReduceTime(0.5f);
            Assert.IsFalse(cooldown.IsReady);
            cooldown.ReduceTime(0.5f);
            Assert.IsTrue(cooldown.IsReady);
        }
    }
}