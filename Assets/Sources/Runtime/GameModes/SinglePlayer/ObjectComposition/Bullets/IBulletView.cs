using Model.SpatialObject;

namespace GameModes.SinglePlayer.ObjectComposition.Bullets
{
    public interface IBulletView
    {
        IPositionView PositionView { get; set; }
    }
}