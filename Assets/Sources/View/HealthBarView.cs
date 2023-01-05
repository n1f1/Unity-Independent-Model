using Model.Characters.CharacterHealth;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    [RequireComponent(typeof(Slider))]
    internal class HealthBarView : MonoBehaviour, IHealthView
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void Display(float normalizedHealth)
        {
            _slider.normalizedValue = normalizedHealth;
        }
    }
}