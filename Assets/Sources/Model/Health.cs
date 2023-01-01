using System;

namespace Model
{
    public class Health : IDamageable
    {
        private const float MaxHealth = 100f;
        
        private readonly IHealthView _healthView;

        public Health(IHealthView healthView)
        {
            _healthView = healthView;
        }

        public float Amount { get; private set; } = MaxHealth;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentException();
            
            Amount = Math.Clamp(Amount - damage, 0, Amount);
            _healthView.Display(Amount / MaxHealth);
        }
    }
}