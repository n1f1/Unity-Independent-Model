using Model.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.SpatialObject;
using UnityEngine;

namespace Simulation.Shooting.Bullets
{
    class BulletView : MonoBehaviour, IBulletView
    {
        public IPositionView PositionView { get; set; }

        private void Awake()
        {
            PositionView = gameObject.AddComponent<PositionView>();
        }
    }
}