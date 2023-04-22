using System;

namespace Model.Characters.CharacterHealth
{
    public class Health : IDamageable
    {
        private readonly IHealthView _healthView;
        private readonly IDeath _death;
        private readonly float _maxHealth;

        public Health(float maxHealth, IHealthView healthView, IDeath death)
        {
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException();

            _maxHealth = maxHealth;
            Amount = maxHealth;
            _healthView = healthView ?? throw new ArgumentNullException();
            _death = death ?? throw new ArgumentNullException();
            _healthView.Display(1);
        }

        public float Amount { get; private set; }

        public bool CanTakeDamage() =>
            Amount > 0;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException();

            if (CanTakeDamage() == false)
                throw new InvalidOperationException();

            Amount = Math.Clamp(Amount - damage, 0, Amount);
            _healthView.Display(Amount / _maxHealth);

            if (Amount == 0)
                _death.Die();
        }
    }
}