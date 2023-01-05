namespace Simulation.Input
{
    internal class AxisInput : IMovementInput
    {
        public float X => UnityEngine.Input.GetAxis("Horizontal");
        public float Y => UnityEngine.Input.GetAxis("Vertical");
    }
}