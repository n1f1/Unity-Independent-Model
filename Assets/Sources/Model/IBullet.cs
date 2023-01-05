namespace Model
{
    public interface IBullet
    {
        void AddPassedTime(float deltaTime);
        void Hit(IDamageable damageable);
    }
}