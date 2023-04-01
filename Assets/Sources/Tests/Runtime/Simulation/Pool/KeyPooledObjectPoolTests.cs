using System;
using NUnit.Framework;
using Simulation.Pool;
using Tests.Simulation.Support;

namespace Tests.Simulation.Pool
{
    public class KeyPooledObjectPoolTests
    {
        [Test]
        public void AddingEnablesPooledObject()
        {
            var pool = new KeyPooledObjectPool<object, TestPoolObject>();
            object key = new object();
            TestPoolObject poolable = new TestPoolObject();
            pool.AddNew(key, poolable);
            Assert.IsTrue(poolable.WasDisabled);
            Assert.IsFalse(poolable.WasEnabled);
        }

        [Test]
        public void CanNotAddTwice()
        {
            var pool = new KeyPooledObjectPool<object, TestPoolObject>();
            object key = new object();
            TestPoolObject poolable = new TestPoolObject();
            pool.AddNew(key, poolable);
            Assert.Throws<InvalidOperationException>(() => pool.AddNew(key, poolable));
            pool.GetFreeByKey();
            Assert.Throws<InvalidOperationException>(() => pool.AddNew(key, poolable));
        }

        [Test]
        public void GettingEnablesPooledObject()
        {
            var pool = new KeyPooledObjectPool<object, TestPoolObject>();
            object key = new object();
            TestPoolObject poolable = new TestPoolObject();
            pool.AddNew(key, poolable);
            pool.GetFreeByKey();
            Assert.True(poolable.WasEnabled);
        }

        [Test]
        public void CanNotGetRemoved()
        {
            var pool = new KeyPooledObjectPool<object, TestPoolObject>();
            object key = new object();
            TestPoolObject poolable = new TestPoolObject();
            pool.AddNew(key, poolable);
            pool.GetFreeByKey();
            pool.RemoveActive(key);
            Assert.Throws<InvalidOperationException>(() => pool.GetFreeByKey());
        }

        [Test]
        public void CanGetReturned()
        {
            var pool = new KeyPooledObjectPool<object, TestPoolObject>();
            object key = new object();
            TestPoolObject poolable = new TestPoolObject();
            pool.AddNew(key, poolable);
            pool.GetFreeByKey();
            pool.ReturnInactive(key);
            Assert.DoesNotThrow(() => pool.GetFreeByKey());
        }
    }
}