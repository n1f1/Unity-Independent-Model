using Model.Characters.CharacterHealth;

namespace Model.Shooting
{
    public interface IShooter
    {
        bool CanHit(IDamageable damageable);
    }
}