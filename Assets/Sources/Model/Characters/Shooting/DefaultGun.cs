using System;
using Model.Characters.Shooting.Bullets;

namespace Model.Characters.Shooting
{
    public class DefaultGun : IWeapon
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;

        public DefaultGun(IAim aim, IBulletFactory<IBullet> bulletFactory)
        {
            _bulletFactory = bulletFactory ?? throw new ArgumentException();
            Aim = aim ?? throw new ArgumentException();
        }

        public IAim Aim { get; }

        public void Shoot() =>
            _bulletFactory.CreateBullet(Aim.Trajectory, 25f, 5);
    }
}