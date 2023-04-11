using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Simulation.Characters.CharacterHealth;
using Simulation.SpatialObject;

namespace Simulation.Characters.Player
{
    public class PlayerView : CharacterWithHealthView, IPlayerView
    {
        public IForwardAimView ForwardAimView { get; set; }

        private void Awake()
        {
            //TODO: editor assertions
            PositionView = gameObject.AddComponent<PositionView>();
            HealthView = gameObject.GetComponentInChildren<IHealthView>();
            ForwardAimView = gameObject.GetComponentInChildren<IForwardAimView>();
        }
    }
}