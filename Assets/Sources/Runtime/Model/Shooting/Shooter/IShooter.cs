using Model.Characters.CharacterHealth;

namespace Model.Shooting.Shooter
{
    public interface IShooter
    {
        bool CanHit(IDamageable damageable);
    }
}