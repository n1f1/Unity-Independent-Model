using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.SpatialObject;

namespace GameModes.SinglePlayer.ObjectComposition
{
    public interface IPlayerView
    {
        IPositionView PositionView { get; set; }
        IHealthView HealthView { get; set; }
        IForwardAimView ForwardAimView { get; set; }
    }
}