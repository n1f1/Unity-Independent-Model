using System;

namespace Model.Shooting
{
    public class Cooldown
    {
        private readonly float _cooldown;
        private float _currentTime;

        public Cooldown(float cooldown)
        {
            if (cooldown < 0)
                throw new ArgumentOutOfRangeException();

            _cooldown = cooldown;
            _currentTime = _cooldown;
        }

        public bool IsReady => _currentTime == 0;

        public void ReduceTime(float deltaTime)
        {
            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException();

            _currentTime = Math.Clamp(_currentTime - deltaTime, 0, _cooldown);
        }

        public void Reset()
        {
            _currentTime = _cooldown;
        }
    }
}