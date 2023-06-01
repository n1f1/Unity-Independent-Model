using System;

namespace Model.Characters.CharacterHealth
{
    public class CompositeDeath : IDeathView
    {
        private readonly IDeathView[] _deathViews;

        public CompositeDeath(params IDeathView[] deathViews)
        {
            _deathViews = deathViews ?? throw new ArgumentNullException(nameof(deathViews));
        }

        public void Die()
        {
            foreach (IDeathView deathView in _deathViews)
                deathView.Die();
        }
    }
}