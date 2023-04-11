using Model.Characters.CharacterHealth;
using Model.Shooting.Bullets;
using Model.SpatialObject;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Shooting
{
    internal class NullBullet : IBullet
    {
        public void Hit(IDamageable damageable)
        {
        }

        public bool Collided { get; }
        public Transform Transform { get; }
    }
}