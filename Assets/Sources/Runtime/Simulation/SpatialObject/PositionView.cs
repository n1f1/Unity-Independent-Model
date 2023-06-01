using Model.SpatialObject;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Simulation.SpatialObject
{
    internal class PositionView : MonoBehaviour, IPositionView
    {
        private UnityEngine.Vector3 _transformPosition = UnityEngine.Vector3.zero;

        public void UpdatePosition(Vector3 position)
        {
            _transformPosition.x = position.X;
            _transformPosition.y = position.Y;
            _transformPosition.z = position.Z;

            transform.position = _transformPosition;
        }
    }
}