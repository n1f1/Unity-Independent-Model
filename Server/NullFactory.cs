using System.Numerics;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using ObjectComposition;

namespace Server
{
    internal class NullPlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(Vector3 position)
        {
            return new Player(new NullPositionVew(), new NullHealthView(), new ForwardAim(new NullAimView()),
                new NullBulletDestroyer(), new NullBulletFactory(), position, new NullDeathView());
        }
    }
}