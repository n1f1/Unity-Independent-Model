using UnityEngine;

internal class AxisInput : IMovementInput
{
    public float X => Input.GetAxis("Horizontal");
    public float Y => Input.GetAxis("Vertical");
}