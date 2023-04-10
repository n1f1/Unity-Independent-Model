using System;

namespace Model.Characters.CharacterHealth
{
    public class Health : IDamageable
    {
        private readonly IHealthView _healthView;
        private readonly IDeath _death;
        private readonly float _maxHealth;
        private float _amount;

        public Health(float maxHealth, IHealthView healthView, IDeath death)
        {
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException();

            _maxHealth = maxHealth;
            _amount = maxHealth;
            _healthView = healthView ?? throw new ArgumentNullException();
            _death = death ?? throw new ArgumentNullException();
            _healthView.Display(1);
        }

        public bool CanTakeDamage() =>
            _amount > 0;

        public void TakeDamage(float damage)
        {
            Console.WriteLine("take damage");
            
            if (damage < 0)
                throw new ArgumentOutOfRangeException();

            if (CanTakeDamage() == false)
                throw new InvalidOperationException();

            _amount = Math.Clamp(_amount - damage, 0, _amount);
            _healthView.Display(_amount / _maxHealth);

            if (_amount == 0)
                _death.Die();
        }
    }
}