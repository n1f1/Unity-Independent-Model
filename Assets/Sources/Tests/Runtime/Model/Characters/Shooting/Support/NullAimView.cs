using System.Numerics;
using Model.Characters.Shooting;

namespace Tests.Model.Characters.Shooting.Support
{
    public class NullAimView : IAimView
    {
        public void Aim(Vector3 position, Vector3 aimPosition)
        {
        }

        public void Stop()
        {
        }
    }
}