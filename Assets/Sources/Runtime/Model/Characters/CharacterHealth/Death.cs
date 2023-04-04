using System;

namespace Model.Characters.CharacterHealth
{
    public class Death : IDeath
    {
        private readonly IDeathView _deathView;

        public Death(IDeathView deathView)
        {
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
        }

        public bool Dead { get; private set; }

        public void Die()
        {
            Dead = true;
            _deathView.Die();
        }
    }
}