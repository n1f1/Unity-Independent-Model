using Model.Characters.Shooting.Bullets;

namespace Server
{
    internal class NullBulletDestroyer : IBulletDestroyer
    {
        public void Destroy(IBullet bullet)
        {
        }
    }
}