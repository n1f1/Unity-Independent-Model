using System;
using Model;
using Model.Characters.CharacterHealth;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Health
{
    public class FakeHealthView : IHealthView, IUpdatable
    {
        private readonly IHealthView _healthView;
        private float _targetValue;
        private float _timeToUpdateView;
        private float _realUpdateDelay = 0.2f;

        public FakeHealthView(IHealthView healthView)
        {
            _healthView = healthView ?? throw new ArgumentNullException(nameof(healthView));
        }

        public void UpdateTime(float time)
        {
            _timeToUpdateView -= time;

            if (_timeToUpdateView <= 0)
                _healthView.Display(_targetValue);
        }
        
        public void Display(float normalizedHealth)
        {
            if(normalizedHealth == _targetValue)
                _healthView.Display(normalizedHealth);
            else
                _timeToUpdateView = _realUpdateDelay;
        }

        public float NormalizedHealth => _healthView.NormalizedHealth;

        public void DisplayFake(float target)
        {
            _targetValue = target;
            _healthView.Display(_targetValue);
        }
    }
}