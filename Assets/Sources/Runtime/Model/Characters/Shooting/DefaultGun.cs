using System;
using Model.Characters.Shooting.Bullets;

namespace Model.Characters.Shooting
{
    public class DefaultGun : IWeapon
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly Cooldown _cooldown;
        private readonly BulletsContainer _bulletsContainer;

        public DefaultGun(IAim aim, IBulletFactory<IBullet> bulletFactory, Cooldown cooldown, BulletsContainer bulletsContainer)
        {
            _cooldown = cooldown ?? throw new ArgumentNullException();
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException();
            Aim = aim ?? throw new ArgumentNullException();
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException();
        }

        public IAim Aim { get; }

        public void Shoot()
        {
            IBullet bullet = _bulletFactory.CreateBullet(Aim.Trajectory, 10f, 5);
            _bulletsContainer.Add(bullet);
            _cooldown.Reset();
        }

        public bool CanShoot() => 
            Aim.Aiming && _cooldown.IsReady;
    }
}