using Model;
using UnityEngine;
using UnityEngine.UI;

namespace SceneViewSimulation
{
    [RequireComponent(typeof(Slider))]
    internal class HealthBar : MonoBehaviour, IHealthView
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