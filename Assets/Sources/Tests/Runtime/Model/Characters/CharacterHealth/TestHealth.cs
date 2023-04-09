using System;
using Model.Characters.CharacterHealth;
using NUnit.Framework;
using Tests.Model.Characters.CharacterHealth.Support;

namespace Tests.Model.Characters.CharacterHealth
{
    public class TestHealth
    {
        [Test]
        public void CanNotInitializeInInvalidState()
        {
            Health health = new Health(100, new NullHealthVew(), new Death(new NullDeathView()));
            Assert.True(health.CanTakeDamage());
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Health(0f, new NullHealthVew(), new Death(new NullDeathView())));
        }

        [Test]
        public void CanTakeOnlyValidDamage()
        {
            Health health = new Health(100, new NullHealthVew(), new Death(new NullDeathView()));
            health.TakeDamage(50);
            Assert.True(health.CanTakeDamage());
            health.TakeDamage(50);
            Assert.False(health.CanTakeDamage());
            Assert.Throws<InvalidOperationException>(() => health.TakeDamage(50));
            Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-50));
        }

        [Test]
        public void DiesInTime()
        {
            TestDeathStatus testDeathStatus = new TestDeathStatus();
            Health health = new Health(100, new NullHealthVew(), testDeathStatus);
            health.TakeDamage(50);
            Assert.False(testDeathStatus.Dead);
            health.TakeDamage(50);
            Assert.True(testDeathStatus.Dead);
        }
    }
}