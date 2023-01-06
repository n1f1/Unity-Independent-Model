using System;
using Model.Characters.CharacterHealth;
using NUnit.Framework;

namespace Tests
{
    public class TestHealth
    {
        [Test]
        public void Test_Construction()
        {
            Health health = new Health(100, new NullHealthVew(), new Death());
            Assert.True(health.CanTakeDamage());
            Assert.Throws<ArgumentOutOfRangeException>(() => new Health(0f, new NullHealthVew(), new Death()));
        }

        [Test]
        public void Test_TakeDamage()
        {
            Health health = new Health(100, new NullHealthVew(), new Death());
            health.TakeDamage(50);
            Assert.True(health.CanTakeDamage());
            health.TakeDamage(50);
            Assert.False(health.CanTakeDamage());
            Assert.Throws<InvalidOperationException>(() => health.TakeDamage(50));
            Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-50));
        }
        
        [Test]
        public void Test_DieWhenZeroHealth()
        {
            TestDeathStatus testDeathStatus = new TestDeathStatus();
            Health health = new Health(100, new NullHealthVew(), testDeathStatus);
            health.TakeDamage(50);
            Assert.False(testDeathStatus.Dead);
            health.TakeDamage(50);
            Assert.True(testDeathStatus.Dead);
        }

        private class TestDeathStatus : IDeath
        {
            public bool Dead { get; private set; }

            public void Die()
            {
                Dead = true;
            }
        }

        private class NullHealthVew : IHealthView
        {
            public void Display(float normalizedHealth)
            {
            
            }
        }
    }
}