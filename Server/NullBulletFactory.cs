using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;

namespace Server
{
    internal class NullBulletFactory : IBulletFactory<IBullet>
    {
        public IBullet CreateBullet(ITrajectory trajectory, float speed, int damage)
        {
            return new NullBullet();
        }

        public void Destroy(IBullet bullet)
        {
            
        }
    }
}