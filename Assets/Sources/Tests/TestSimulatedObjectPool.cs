using System;
using NUnit.Framework;
using Simulation.Common;
using UnityEngine;

namespace Tests
{
    public class TestSimulatedObjectPool
    {
        private SimulatedObjectPool<TestPoolObject> _objectPool;

        [SetUp]
        public void Setup()
        {
            _objectPool = new SimulatedObjectPool<TestPoolObject>();
        }
        
        [Test]
        public void Test_AddNewValid()
        {
            GameObject gameObject = new GameObject();
            _objectPool.AddNew(new TestPoolObject(), gameObject);
            Assert.False(gameObject.activeSelf);
            Assert.True(_objectPool.CanGet());
        }
        
        [Test]
        public void Test_AddNewInvalid()
        {
            Assert.Throws<ArgumentException>(() => _objectPool.AddNew(new TestPoolObject(), null));
            Assert.Throws<ArgumentException>(() => _objectPool.AddNew(null, new GameObject()));
        }
    }

    public class TestPoolObject
    {
    }
}