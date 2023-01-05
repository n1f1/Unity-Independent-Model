using Model.Characters.CharacterHealth;

namespace Model.Characters.Shooting.Bullets
{
    public interface IBullet
    {
        void AddPassedTime(float deltaTime);
        void Hit(IDamageable damageable);
    }
}