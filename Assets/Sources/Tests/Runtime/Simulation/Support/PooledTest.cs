using Simulation.Pool;

namespace Tests.Simulation.Support
{
    public class TestPoolObject : IPoolable
    {
        public bool WasEnabled { get; set; }
        public bool WasDisabled { get; set; }

        public void Enable()
        {
            WasEnabled = true;
        }

        public void Disable()
        {
            WasDisabled = true;
        }
    }
}