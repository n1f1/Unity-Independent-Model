using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;
using Simulation.View;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        private void Awake()
        {
            //TODO: editor assertions
            PositionView = gameObject.AddComponent<PositionView>();
            HealthView = gameObject.GetComponentInChildren<IHealthView>();
            ForwardAimView = gameObject.GetComponentInChildren<IForwardAimView>();
        }

        public IPositionView PositionView { get; set; }
        public IHealthView HealthView { get; set; }
        public IForwardAimView ForwardAimView { get; set; }
    }
}