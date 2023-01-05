using Model;
using Model.Characters.Shooting.Bullets;

namespace Simulation.Shooting
{
    internal class RemoveFromBulletContainer : IBulletDestroyer
    {
        private readonly BulletsContainer _bulletsContainer;
        private readonly IBulletDestroyer _bulletDestroyer;

        public RemoveFromBulletContainer(BulletsContainer bulletsContainer, IBulletDestroyer bulletDestroyer)
        {
            _bulletDestroyer = bulletDestroyer;
            _bulletsContainer = bulletsContainer;
        }

        public void Destroy(IBullet bullet)
        {
            _bulletDestroyer.Destroy(bullet);
            _bulletsContainer.Remove(bullet);
        }
    }
}