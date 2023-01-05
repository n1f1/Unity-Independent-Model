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
            _updatableContainer = updatableContainer;
            _bulletDestroyer = bulletDestroyer;
        }

        public void Destroy(IBullet bullet)
        {
            _updatableContainer.QueryRemove(bullet);
            _bulletDestroyer.Destroy(bullet);
        }
    }
}