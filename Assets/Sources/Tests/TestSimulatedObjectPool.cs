using System;
using NUnit.Framework;
using Simulation.Common;

namespace Tests
{
    public class TestSimulatedObjectPool
    {
        [Test]
        public void Test_AddNewValid()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable> objectPool = new();
            TestPoolable poolable = new TestPoolable();
            objectPool.AddNew(new TestPoolableType(), poolable);
            Assert.False(poolable.Enabled);
            Assert.True(objectPool.CanGet());
        }
        
        [Test]
        public void Test_GetValid()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable>objectPool = new();
            TestPoolableType testPoolableType = new TestPoolableType();
            TestPoolable poolable = new TestPoolable();
            objectPool.AddNew(testPoolableType, poolable);
            KeyValueObjectPool<TestPoolableType, TestPoolable>.SimulatedPair simulatedPair = objectPool.Get();
            
            Assert.True(simulatedPair.Poolable == poolable);
            Assert.True(poolable.Enabled);
            Assert.True(simulatedPair.TObject == testPoolableType);
        }
        
        [Test]
        public void Test_CanGet()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable> objectPool = new();

            Assert.False(objectPool.CanGet());

            objectPool.AddNew(new TestPoolableType(), new TestPoolable());
            
            Assert.True(objectPool.CanGet());
        }
        
        [Test]
        public void Test_CannotGetFail()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable>objectPool = new();

            Assert.Throws<InvalidOperationException>(() => objectPool.Get());
        }
        
        [Test]
        public void Test_AddNewInvalid()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable> objectPool = new();

            Assert.Throws<ArgumentException>(() => objectPool.AddNew(new TestPoolableType(), null));
            Assert.Throws<ArgumentException>(() => objectPool.AddNew(null, new TestPoolable()));
        }
        
        [Test]
        public void Test_Return()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable> objectPool = new();
            TestPoolableType poolableType = new TestPoolableType();
            TestPoolable poolable = new TestPoolable();
            objectPool.AddNew(poolableType, poolable);
            objectPool.Get();

            Assert.Throws<ArgumentNullException>(()=> objectPool.Return(null));
            Assert.Throws<InvalidOperationException>(()=> objectPool.Return(new TestPoolableType()));
            
            objectPool.Return(poolableType);
            
            Assert.True(objectPool.CanGet());
            Assert.False(poolable.Enabled);
        }
        
        [Test]
        public void Test_Replace()
        {
            KeyValueObjectPool<TestPoolableType, TestPoolable> objectPool = new();
            TestPoolableType poolableType = new TestPoolableType();
            TestPoolable poolable = new TestPoolable();
            objectPool.AddNew(poolableType, poolable);
            objectPool.Get();
            
            TestPoolableType second = new TestPoolableType();
            objectPool.Replace(poolableType, second);

            Assert.Throws<InvalidOperationException>(() => objectPool.Return(poolableType));
            Assert.DoesNotThrow(() => objectPool.Return(second));
        }
    }

    public class TestPoolable : IPoolable
    {
        public bool Enabled { get; private set; }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }
    }

    public class TestPoolableType
    {
    }
}