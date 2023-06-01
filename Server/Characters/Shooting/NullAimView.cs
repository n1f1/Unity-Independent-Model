using System.Numerics;
using Model.Shooting;

namespace Server.Characters.Shooting
{
    internal class NullAimView : IAimView
    {
        public void Aim(Vector3 position, Vector3 aimPosition)
        {
        }

        public void Stop()
        {
        }
    }
}