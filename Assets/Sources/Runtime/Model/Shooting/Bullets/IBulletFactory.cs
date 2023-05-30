using Model.Shooting.Shooter;
using Model.Shooting.Trajectory;

namespace Model.Shooting.Bullets
{
    public interface IBulletFactory<out TBullet>
    {
        TBullet CreateBullet(ITrajectory trajectory, float speed, int damage, IShooter shooter);
    }
}