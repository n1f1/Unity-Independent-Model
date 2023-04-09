using System;
using Model.Characters.Shooting.Bullets;

namespace Model.Characters.Shooting
{
    public class DefaultGun : IWeapon
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly Cooldown _cooldown;
        private readonly BulletsContainer _bulletsContainer;
        private readonly float _bulletSpeed = 10f;
        private readonly int _bulletDamage = 5;

        public DefaultGun(IBulletFactory<IBullet> bulletFactory, Cooldown cooldown,
            BulletsContainer bulletsContainer)
        {
            _cooldown = cooldown ?? throw new ArgumentNullException();
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException();
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException();
        }

        public IBullet Shoot(IAim aim)
        {
            IBullet bullet = _bulletFactory.CreateBullet(aim.Trajectory, _bulletSpeed, _bulletDamage);
            _bulletsContainer.Add(bullet);
            _cooldown.Reset();
            
            return bullet;
        }

        public bool CanShoot(IAim aim) =>
            aim.Aiming && _cooldown.IsReady;
    }
}