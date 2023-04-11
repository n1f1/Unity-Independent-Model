using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.View;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition.Bullets
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