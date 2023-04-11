using Model.Characters.CharacterHealth;

namespace Simulation.Characters.Enemy
{
    public interface IEnemySimulation
    {
        ISimulation<IDamageable> Damageable { get; set; }
    }
}