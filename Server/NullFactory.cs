using System.Numerics;
using Model.Characters;
using Model.Characters.Shooting;

namespace Server
{
    internal class NullPlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(Vector3 position)
        {
            return new Player(new NullPositionVew(), new NullHealthView(), new ForwardAim(new NullAimView()),
                new NullBulletDestroyer(), new NullBulletFactory(), position);
        }
    }
}