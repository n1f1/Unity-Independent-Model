using Model.SpatialObject;

namespace Model.Characters.CharacterHealth
{
    public interface ICharacterWithHealthView
    {
        IPositionView PositionView { get; set; }
        IHealthView HealthView { get; set; }
        IDeathView DeathView { get; set; }
    }
}