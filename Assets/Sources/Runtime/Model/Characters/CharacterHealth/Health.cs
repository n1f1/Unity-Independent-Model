using System;

namespace Model.Characters.CharacterHealth
{
    public class Health : IDamageable
    {
        private readonly IHealthView _healthView;
        private readonly IDeath _death;
        private readonly float _maxHealth;

        public Health(float health, float maxHealth, IHealthView healthView, IDeath death)
        {
            if (maxHealth <= 0 || health <= 0 || health > maxHealth)
                throw new ArgumentOutOfRangeException();

            _maxHealth = maxHealth;
            Amount = health;
            _healthView = healthView ?? throw new ArgumentNullException(nameof(healthView));
            _death = death ?? throw new ArgumentNullException(nameof(death));
            Display();
        }

        public float Amount { get; private set; }
        public bool Dead => Amount == 0;

        public bool CanTakeDamage() =>
            Amount > 0;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException();

            if (CanTakeDamage() == false)
                throw new InvalidOperationException();

            Amount = Math.Clamp(Amount - damage, 0, Amount);
            Display();

            if (Amount == 0)
                _death.Die();
        }

        private void Display() => 
            _healthView.Display(Amount / _maxHealth);
    }
}