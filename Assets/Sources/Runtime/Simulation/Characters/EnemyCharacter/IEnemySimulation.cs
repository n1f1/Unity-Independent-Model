using Model.Characters.CharacterHealth;

namespace Simulation.Characters.EnemyCharacter
{
    public interface IEnemySimulation
    {
        ISimulation<IDamageable> Damageable { get; set; }
    }
}