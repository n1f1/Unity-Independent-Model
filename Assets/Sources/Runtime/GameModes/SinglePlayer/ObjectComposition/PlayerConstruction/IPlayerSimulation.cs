using Model;
using Model.Characters;
using Model.Characters.Shooting;

namespace GameModes.SinglePlayer.ObjectComposition.PlayerConstruction
{
    public interface IPlayerSimulation
    {
        ISimulation<IMovable> Movable { get; set; }
        ISimulation<ICharacterShooter> CharacterShooter { get; set; }
    }
}