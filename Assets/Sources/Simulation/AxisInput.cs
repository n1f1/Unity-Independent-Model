using UnityEngine;

namespace Simulation
{
    internal class AxisInput : IMovementInput
    {
        public float X => Input.GetAxis("Horizontal");
        public float Y => Input.GetAxis("Vertical");
    }
}