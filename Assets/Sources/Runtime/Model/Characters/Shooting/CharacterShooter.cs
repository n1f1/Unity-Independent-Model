using System;
using System.Numerics;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;

namespace Model.Characters.Shooting
{
    public class CharacterShooter : ICharacterShooter
    {
        private readonly IWeapon _weapon;
        private readonly Transform _character;
        private readonly IAim _aim;

        public CharacterShooter(IAim aim, IWeapon weapon, Transform character)
        {
            _aim = aim ?? throw new ArgumentNullException(nameof(aim));
            _weapon = weapon ?? throw new ArgumentNullException();
            _character = character ?? throw new ArgumentNullException();
        }

        public void AimAt(Vector3 aimPosition)
        {
            Aim(_character.Position, aimPosition);
        }

        public void Aim(Vector3 from, Vector3 aimPosition)
        {
            _aim.Aim(from, aimPosition);
        }

        public IBullet Shoot()
        {
            if (_weapon.CanShoot(_aim) == false)
                throw new InvalidOperationException();
                
            return _weapon.Shoot(_aim);
        }

        public bool CanShoot() => 
            _weapon.CanShoot(_aim);

        public void StopAiming()
        {
            _aim.Stop();
        }
    }
}