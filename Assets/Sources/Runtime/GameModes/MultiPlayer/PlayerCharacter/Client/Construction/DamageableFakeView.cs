using System;
using Model.Characters.CharacterHealth;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class DamageableFakeView : IDamageable
    {
        private readonly IHealthView _view;
        private readonly int _maxHealth;
        private float _target;

        public DamageableFakeView(int maxHealth, float healthAmount, IHealthView view)
        {
            _maxHealth = maxHealth;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _target = healthAmount / maxHealth;
            Display();
        }

        public void TakeDamage(float damage)
        {
            _target -= damage / _maxHealth;
            _view.Display(_target);
        }

        public bool CanTakeDamage()
        {
            return true;
        }

        private void Display()
        {
            _view.Display(_target);
        }
    }
}