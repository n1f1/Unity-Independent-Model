using System;
using Model.Characters.Shooting.Bullets;
using Simulation.Common;

namespace Simulation.Shooting
{
    internal class RemoveFromBulletContainer : IBulletDestroyer
    {
        private readonly IBulletDestroyer _bulletDestroyer;
        private readonly UpdatableContainer _updatableContainer;

        public RemoveFromBulletContainer(UpdatableContainer updatableContainer, IBulletDestroyer bulletDestroyer)
        {
            _updatableContainer = updatableContainer ?? throw new ArgumentException();
            _bulletDestroyer = bulletDestroyer ?? throw new ArgumentException();
        }

        public void Destroy(IBullet bullet)
        {
            _updatableContainer.QueryRemove(bullet);
            _bulletDestroyer.Destroy(bullet);
        }
    }
}