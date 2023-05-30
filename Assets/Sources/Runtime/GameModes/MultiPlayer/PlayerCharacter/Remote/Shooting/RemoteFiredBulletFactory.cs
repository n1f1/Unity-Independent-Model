using System;
using System.Numerics;
using Model.Shooting.Bullets;
using Model.Shooting.Shooter;
using Model.Shooting.Trajectory;
using Model.Shooting.Trajectory.Bezier;
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
            ITrajectory newTrajectory = GetTrajectoryWithPredictionAdjust(trajectory, speed);
            IBullet bullet = _bulletFactory.CreateBullet(newTrajectory, speed, damage, shooter);

            return new RemoteFiredBullet(bullet);
        }

        private ITrajectory GetTrajectoryWithPredictionAdjust(ITrajectory trajectory, float speed)
        {
            Vector3 startPosition = _playerTransform.Position;
            startPosition.Y = trajectory.Start.Y;

            Vector3 fakeFinish = trajectory.EvaluateForNormalizedRatio(speed / trajectory.Distance);
            Vector3 curvePoint = trajectory.EvaluateForNormalizedRatio(speed / 2f / trajectory.Distance);
            ITrajectory fakeAdjustmentTrajectory = new BezierTrajectory(startPosition, curvePoint, fakeFinish, speed);

            ITrajectory remainingRealTrajectory = new ForwardTrajectory(fakeFinish, trajectory.Finish);
            ITrajectory totalTrajectory = new CompositeTrajectory(fakeAdjustmentTrajectory, remainingRealTrajectory);

            return totalTrajectory;
        }
    }
}