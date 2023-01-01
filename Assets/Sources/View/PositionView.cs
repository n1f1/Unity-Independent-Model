using Model;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace View
{
    internal class PositionView : MonoBehaviour, IPositionView
    {
        public void UpdatePosition(Vector3 position) => 
            transform.position = new UnityEngine.Vector3(position.X, position.Y, position.Z);
    }
}