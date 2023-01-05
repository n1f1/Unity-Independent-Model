using System;
using Model.Characters.Shooting.Bullets;

namespace Model.Characters.Shooting
{
    public class DefaultGun : IWeapon
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly Cooldown _cooldown;

        public DefaultGun(IAim aim, IBulletFactory<IBullet> bulletFactory, Cooldown cooldown)
        {
            _cooldown = cooldown ?? throw new ArgumentException();
            _bulletFactory = bulletFactory ?? throw new ArgumentException();
            Aim = aim ?? throw new ArgumentException();
        }

        public IAim Aim { get; }

        public void Shoot()
        {
            _bulletFactory.CreateBullet(Aim.Trajectory, 10f, 5);
            _cooldown.Reset();
        }

        public bool CanShoot() => 
            Aim.Aiming && _cooldown.IsReady;
    }
}