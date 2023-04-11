using System.Numerics;
using Model.Shooting.Bullets;

namespace Model.Characters.Player
{
    public interface ICharacterShooter
    {
        void AimAt(Vector3 aimPosition);
        IBullet Shoot();
        void StopAiming();
        bool CanShoot();
    }
}