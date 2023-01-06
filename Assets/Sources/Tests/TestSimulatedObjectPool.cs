using System;
using NUnit.Framework;
using Simulation.Common;
using UnityEngine;

namespace Tests
{
    public class TestSimulatedObjectPool
    {
        [Test]
        public void Test_AddNewValid()
        {
            SimulatedObjectPool<TestPoolObject> objectPool = new();
            GameObject gameObject = new GameObject();
            objectPool.AddNew(new TestPoolObject(), gameObject);
            Assert.False(gameObject.activeSelf);
            Assert.True(objectPool.CanGet());
        }
        
        [Test]
        public void Test_GetValid()
        {
            SimulatedObjectPool<TestPoolObject> objectPool = new();
            GameObject gameObject = new GameObject();
            TestPoolObject testPoolObject = new TestPoolObject();
            objectPool.AddNew(testPoolObject, gameObject);
            SimulatedObjectPool<TestPoolObject>.SimulatedPair simulatedPair = objectPool.Get();
            
            Assert.True(simulatedPair.GameObject == gameObject);
            Assert.True(simulatedPair.GameObject.activeSelf);
            Assert.True(simulatedPair.TObject == testPoolObject);
        }
        
        [Test]
        public void Test_CanGet()
        {
            SimulatedObjectPool<TestPoolObject> objectPool = new();

            Assert.False(objectPool.CanGet());

            objectPool.AddNew(new TestPoolObject(), new GameObject());
            
            Assert.True(objectPool.CanGet());
        }
        
        [Test]
        public void Test_CannotGetFail()
        {
            SimulatedObjectPool<TestPoolObject> objectPool = new();

            Assert.Throws<InvalidOperationException>(() => objectPool.Get());
        }
        
        [Test]
        public void Test_AddNewInvalid()
        {
            SimulatedObjectPool<TestPoolObject> objectPool = new();

            Assert.Throws<ArgumentException>(() => objectPool.AddNew(new TestPoolObject(), null));
            Assert.Throws<ArgumentException>(() => objectPool.AddNew(null, new GameObject()));
        }
        
        [Test]
        public void Test_Return()
        {
            SimulatedObjectPool<TestPoolObject> objectPool = new();
            TestPoolObject poolObject = new TestPoolObject();
            GameObject gameObject = new GameObject();
            objectPool.AddNew(poolObject, gameObject);
            objectPool.Get();

            Assert.Throws<ArgumentException>(()=> objectPool.Return(null));
            Assert.Throws<ArgumentException>(()=> objectPool.Return(new TestPoolObject()));
            
            objectPool.Return(poolObject);
            
            Assert.True(objectPool.CanGet());
            Assert.False(gameObject.activeSelf);
        }
    }

    public class TestPoolObject
    {
    }
}