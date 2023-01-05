namespace Model
{
    public class DefaultGun : IWeapon
    {
        private IBulletFactory<IBullet> _bulletFactory;

        public DefaultGun(IAim aim, IBulletFactory<IBullet> bulletFactory)
        {
            _bulletFactory = bulletFactory;
            Aim = aim;
        }

        public IAim Aim { get; }

        public void Shoot()
        {
            IBullet bullet = _bulletFactory.CreateBullet(Aim.Trajectory, 25f, 5);
        }
    }
}