using System;
using Model;
using NUnit.Framework;
using Simulation.Common;

namespace Tests
{
    public class TestUpdatableContainer
    {
        [Test]
        public void Test_UpdateFail()
        {
            UpdatableContainer container = new UpdatableContainer();
            Assert.Throws<ArgumentException>(() => container.UpdateTime(-1));
        }
        
        [Test]
        public void Test_Update_Updates()
        {
            UpdatableContainer container = new UpdatableContainer();
            UpdatableWithStatus updatable = new UpdatableWithStatus();
            container.QueryAdd(updatable);
            Assert.False(updatable.Updated);
            container.UpdateTime(1f);
            Assert.True(updatable.Updated);
        }
        
        [Test]
        public void Test_AddRemove()
        {
            UpdatableContainer container = new UpdatableContainer();
            UpdatableWithStatus updatable = new UpdatableWithStatus();
            Assert.DoesNotThrow(() => container.QueryAdd(updatable));
            Assert.DoesNotThrow(() => container.QueryRemove(updatable));
            Assert.Throws<ArgumentException>(() => container.QueryAdd(null));
            Assert.Throws<ArgumentException>(() => container.QueryRemove(null));
        }
        
        [Test]
        public void Test_AddRemove_DelayedWhenUpdates()
        {
            UpdatableContainer container = new UpdatableContainer();
            UpdatableWithStatus updatableWithStatus = new UpdatableWithStatus();
            UpdatableAddingOnUpdate updatable = new UpdatableAddingOnUpdate(container, updatableWithStatus);
            container.QueryAdd(updatable);
            Assert.DoesNotThrow(() => container.UpdateTime(1f));
            Assert.DoesNotThrow(() => container.QueryRemove(updatableWithStatus));
        }

        private class UpdatableAddingOnUpdate : IUpdatable
        {
            private readonly UpdatableContainer _container;
            private readonly UpdatableWithStatus _updatableWithStatus;

            public UpdatableAddingOnUpdate(UpdatableContainer container, UpdatableWithStatus updatableWithStatus)
            {
                _updatableWithStatus = updatableWithStatus;
                _container = container;
            }

            public void UpdateTime(float deltaTime)
            {
                _container.QueryAdd(_updatableWithStatus);
            }
        }

        private class UpdatableWithStatus : IUpdatable
        {
            public bool Updated { get; private set; }

            public void UpdateTime(float deltaTime)
            {
                Updated = true;
            }
        }
    }
}