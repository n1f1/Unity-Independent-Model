using Model.SpatialObject;

namespace Model.Shooting.Bullets
{
    public interface IBulletView
    {
        IPositionView PositionView { get; set; }
    }
}