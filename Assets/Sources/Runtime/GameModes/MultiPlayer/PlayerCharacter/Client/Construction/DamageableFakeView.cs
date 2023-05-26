using System;
using Model.Characters.CharacterHealth;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class DamageableFakeView : IDamageable
    {
        private readonly IHealthView _view;
        private readonly int _maxHealth;
        private float _target;

        public DamageableFakeView(int maxHealth, IHealthView view)
        {
            _maxHealth = maxHealth;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _target = _view.NormalizedHealth;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log(damage);
            _target -= damage / _maxHealth;
            _view.Display(_target);
        }

        public bool CanTakeDamage()
        {
            return true;
        }
    }
}