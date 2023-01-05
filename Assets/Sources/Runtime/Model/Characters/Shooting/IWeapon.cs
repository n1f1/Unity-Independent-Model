namespace Model.Characters.Shooting
{
    public interface IWeapon
    {
        public IAim Aim { get; }
        public void Shoot();
        bool CanShoot();
    }
}