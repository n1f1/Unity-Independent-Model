using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace Model.Shooting.Bullets
{
    public interface IBullet : IUpdatable
    {
        void Hit(IDamageable damageable);
        bool Collided { get; }
        Transform Transform { get; }
    }
}