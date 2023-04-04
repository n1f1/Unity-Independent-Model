namespace Model.Characters.Shooting.Bullets
{
    public interface IBulletFactory<out TBullet> : IBulletDestroyer where TBullet : IBullet
    {
        TBullet CreateBullet(ITrajectory trajectory, float speed, int damage);
    }
}