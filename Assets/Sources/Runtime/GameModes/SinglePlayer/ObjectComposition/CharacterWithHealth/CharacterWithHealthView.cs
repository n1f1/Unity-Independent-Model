using Model.Characters.CharacterHealth;
using Model.SpatialObject;
using Simulation.View;
using UnityEngine;

namespace GameModes.SinglePlayer.ObjectComposition.CharacterWithHealth
{
    public class CharacterWithHealthView : MonoBehaviour, ICharacterWithHealthView
    {
        private void Awake()
        {
            //TODO: editor assertions
            PositionView = gameObject.AddComponent<PositionView>();
            HealthView = gameObject.GetComponentInChildren<IHealthView>();
        }

        public IPositionView PositionView { get; set; }
        public IHealthView HealthView { get; set; }
    }
}