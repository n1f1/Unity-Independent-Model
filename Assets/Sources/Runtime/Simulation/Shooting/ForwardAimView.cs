using Model.Shooting;
using UnityEngine;
using Utility;
using Vector3 = System.Numerics.Vector3;

namespace Simulation.Shooting
{
    internal class ForwardAimView : MonoBehaviour, IForwardAimView
    {
        [SerializeField] private float _distance;
        [SerializeField] private LineRenderer _lineRenderer;

        public void Aim(Vector3 position, Vector3 aimPosition)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, position.Convert());
            _lineRenderer.SetPosition(1, (position + Vector3.Normalize(aimPosition - position) * _distance).Convert());
        }

        public void Stop()
        {
            _lineRenderer.enabled = false;
        }
    }
}