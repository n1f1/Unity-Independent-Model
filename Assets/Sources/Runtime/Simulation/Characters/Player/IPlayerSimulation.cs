using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;

namespace Simulation.Characters.Player
{
    public interface IPlayerSimulation
    {
        ISimulation<IMovable> Movable { get; set; }
        ISimulation<ICharacterShooter> CharacterShooter { get; set; }
        ISimulation<IDamageable> Damageable { get; set; }
    }
}