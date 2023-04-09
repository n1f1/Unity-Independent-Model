using System.Numerics;
using Model.Characters.Shooting;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    internal class NullAimView : IForwardAimView
    {
        public void Aim(Vector3 position, Vector3 aimPosition)
        {
        }

        public void Stop()
        {
        }
    }
}