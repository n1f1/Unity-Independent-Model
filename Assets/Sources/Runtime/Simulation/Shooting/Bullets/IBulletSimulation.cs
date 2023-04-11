using Model.Characters.CharacterHealth;
using Model.Physics;

namespace Simulation.Shooting.Bullets
{
    public interface IBulletSimulation
    {
        ISimulation<IPhysicsInteraction<Trigger<IDamageable>>> Collidable { get; set; }
    }
}