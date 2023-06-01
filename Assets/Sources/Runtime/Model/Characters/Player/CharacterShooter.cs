using System;
using System.Numerics;
using Model.Shooting;
using Model.SpatialObject;

namespace Model.Characters.Player
{
    public class CharacterShooter : ICharacterShooter
    {
        private readonly Transform _character;

        public CharacterShooter(IAim aim, IWeapon weapon, Transform character)
        {
            Aim = aim ?? throw new ArgumentNullException(nameof(aim));
            Weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            _character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public IWeapon Weapon { get; }
        public IAim Aim { get; }

        public void AimAt(Vector3 aimPosition)
        {
            Aim.Aim(_character.Position, aimPosition);
        }

        public void UpdateTime(float deltaTime)
        {
            Weapon.CoolDown(deltaTime);
        }
    }
}