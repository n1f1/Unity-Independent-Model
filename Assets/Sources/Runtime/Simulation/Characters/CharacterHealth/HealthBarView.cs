using Model.Characters.CharacterHealth;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.Characters.CharacterHealth
{
    [RequireComponent(typeof(Slider))]
    internal class HealthBarView : MonoBehaviour, IHealthView
    {
        private Slider _slider;
        
        public float NormalizedHealth { get; private set; }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void Display(float normalizedHealth)
        {
            NormalizedHealth = _slider.normalizedValue = normalizedHealth;
        }
    }
}