using System;
using System.Numerics;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.Shooting.Shooter;
using Model.SpatialObject;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting
{
    internal class RemoteFiredBulletFactory : IBulletFactory<IBullet>
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly Transform _playerTransform;

        public RemoteFiredBulletFactory(Transform playerTransform, IBulletFactory<IBullet> bulletFactory)
        {
            _playerTransform = playerTransform ?? throw new ArgumentNullException(nameof(playerTransform));
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        }

        public IBullet CreateBullet(ITrajectory trajectory, float speed, int damage, IShooter shooter)
        {
            Vector3 startPosition = _playerTransform.Position;
            startPosition.Y = trajectory.Start.Y;
            ITrajectory fakeTrajectory = new ForwardTrajectory(startPosition, trajectory.Finish);
            IBullet bullet = _bulletFactory.CreateBullet(fakeTrajectory, speed, damage, shooter);

            return new RemoteFiredBullet(bullet);
        }

        public void Destroy(IBullet bullet)
        {
            _bulletFactory.Destroy(bullet);
        }
    }
}