using System.Numerics;
using Model.Shooting;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting
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