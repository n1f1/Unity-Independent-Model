using Model.SpatialObject;

namespace Model.Characters.Shooting.Bullets
{
    public interface IBulletView
    {
        IPositionView PositionView { get; set; }
    }
}