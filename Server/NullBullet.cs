using Model.Characters.CharacterHealth;
using Model.Characters.Shooting.Bullets;

namespace Server
{
    internal class NullBullet : IBullet
    {
        public void Hit(IDamageable damageable)
        {
            
        }

        public bool Collided { get; }
    }
}