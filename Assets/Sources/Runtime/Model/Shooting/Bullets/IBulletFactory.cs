using Model.Shooting.Shooter;

namespace Model.Shooting.Bullets
{
    public interface IBulletFactory<out TBullet>
    {
        TBullet CreateBullet(ITrajectory trajectory, float speed, int damage, IShooter shooter);
    }
}