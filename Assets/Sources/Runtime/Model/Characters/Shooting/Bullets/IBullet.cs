using Model.Characters.CharacterHealth;

namespace Model.Characters.Shooting.Bullets
{
    public interface IBullet : IUpdatable
    {
        void Hit(IDamageable damageable);
    }
}