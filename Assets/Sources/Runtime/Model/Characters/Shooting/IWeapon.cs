using Model.Characters.Shooting.Bullets;

namespace Model.Characters.Shooting
{
    public interface IWeapon
    {
        public IBullet Shoot(IAim aim);
        bool CanShoot(IAim aim);
    }
}