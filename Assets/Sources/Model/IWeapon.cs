namespace Model
{
    public interface IWeapon
    {
        public IAim Aim { get; }
        public void Shoot();
    }
}