using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;

namespace Server
{
    internal class ServerBulletFactory : IBulletFactory<IBullet>
    {
        public IBullet CreateBullet(ITrajectory trajectory, float speed, int damage)
        {
            IBullet bullet = new DefaultBullet(new Transform(new NullPositionVew()), trajectory, speed,damage);
            return bullet;
        }

        public void Destroy(IBullet bullet)
        {
        }
    }
}