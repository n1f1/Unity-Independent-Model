using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace Model.Characters.Shooting.Bullets
{
    public interface IBullet : IUpdatable
    {
        void Hit(IDamageable damageable);
        bool Collided { get; }
        Transform Transform { get; }
    }
}