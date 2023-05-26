using System;
using Model.Characters.CharacterHealth;

namespace Model.Shooting.Shooter
{
    public class CompositeShooter : IShooter
    {
        private readonly IShooter[] _shooters;

        public CompositeShooter(params IShooter[] shooters)
        {
            _shooters = shooters ?? throw new ArgumentNullException(nameof(shooters));
        }

        public bool CanHit(IDamageable damageable)
        {
            foreach (IShooter shooter in _shooters)
            {
                if (shooter.CanHit(damageable) == false)
                    return false;
            }

            return true;
        }
    }
}