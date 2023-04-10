using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace GameModes.SinglePlayer.ObjectComposition.CharacterWithHealth
{
    public interface ICharacterWithHealthView
    {
        IPositionView PositionView { get; set; }
        IHealthView HealthView { get; set; }
    }
}