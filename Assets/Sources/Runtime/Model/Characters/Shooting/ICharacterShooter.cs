using System.Numerics;
using Model.Characters.Shooting.Bullets;

namespace Model.Characters.Shooting
{
    public interface ICharacterShooter
    {
        void AimAt(Vector3 aimPosition);
        IBullet Shoot();
        void StopAiming();
        bool CanShoot();
    }
}