using Model;
using Model.Characters.CharacterHealth;

namespace GameModes.SinglePlayer.ObjectComposition.EnemyConstruction
{
    public interface IEnemySimulation
    {
        ISimulation<IDamageable> Damageable { get; set; }
    }
}