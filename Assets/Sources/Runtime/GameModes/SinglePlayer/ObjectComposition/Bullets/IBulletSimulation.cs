using Model;
using Model.Characters.CharacterHealth;
using Model.Physics;
using Simulation;

namespace GameModes.SinglePlayer.ObjectComposition.Bullets
{
    public interface IBulletSimulation
    {
        ISimulation<IPhysicsInteraction<Trigger<IDamageable>>> Collidable { get; set; }
    }
}