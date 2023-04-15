using Model.Shooting.Bullets;

namespace Model.Shooting
{
    public interface IWeapon
    {
        public IBullet Shoot(IAim aim);
        bool CanShoot(IAim aim);
        void CoolDown(float deltaTime);
    }
}