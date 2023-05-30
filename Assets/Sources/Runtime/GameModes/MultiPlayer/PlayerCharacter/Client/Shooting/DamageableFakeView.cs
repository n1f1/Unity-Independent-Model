using System;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using Model.Characters.CharacterHealth;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class DamageableFakeView : IDamageable
    {
        private readonly FakeHealthView _view;
        private readonly int _maxHealth;
        private float _target;

        public DamageableFakeView(int maxHealth, float healthAmount, FakeHealthView view)
        {
            _maxHealth = maxHealth;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _target = healthAmount / maxHealth;
            Display();
        }

        public void TakeDamage(float damage)
        {
            _target -= damage / _maxHealth;
            Display();
        }

        public bool CanTakeDamage()
        {
            return true;
        }

        private void Display()
        {
            _view.DisplayFake(_target);
        }
    }
}