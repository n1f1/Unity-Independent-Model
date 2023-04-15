using System.Numerics;
using Model.Shooting;
using Model.Shooting.Bullets;

namespace Model.Characters.Player
{
    public interface ICharacterShooter
    {
        IWeapon Weapon { get; }
        IAim Aim { get; }
        void AimAt(Vector3 aimPosition);
    }
}