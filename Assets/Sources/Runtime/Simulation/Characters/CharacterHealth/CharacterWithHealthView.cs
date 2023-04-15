using Model.Characters.CharacterHealth;
using Model.SpatialObject;
using Simulation.SpatialObject;
using UnityEngine;

namespace Simulation.Characters.CharacterHealth
{
    public class CharacterWithHealthView : MonoBehaviour, ICharacterWithHealthView
    {
        public IPositionView PositionView { get; set; }
        public IHealthView HealthView { get; set; }
        public IDeathView DeathView { get; set; }

        private void Awake()
        {
            //TODO: editor assertions
            PositionView = gameObject.AddComponent<PositionView>();
            HealthView = gameObject.GetComponentInChildren<IHealthView>();
            DeathView = new NullDeathView();
        }
    }
}