using System;
using System.Numerics;
using Model.SpatialObject;

namespace Model.Characters.Shooting
{
    public class CharacterShooter
    {
        private readonly IWeapon _weapon;
        private readonly Transform _character;

        public CharacterShooter(IWeapon weapon, Transform character)
        {
            _weapon = weapon ?? throw new ArgumentNullException();
            _character = character ?? throw new ArgumentNullException();
        }

        public void Aim(Vector3 aimPosition)
        {
            _weapon.Aim.Aim(_character.Position, aimPosition);
        }

        public void Shoot()
        {
            if (_weapon.CanShoot())
                _weapon.Shoot();
        }

        public void StopAiming()
        {
            _weapon.Aim.Stop();
        }
    }
}