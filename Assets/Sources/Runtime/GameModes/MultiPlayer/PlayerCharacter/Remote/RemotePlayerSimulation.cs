using GameModes.MultiPlayer.PlayerCharacter.Remote.Movement;
using Simulation;
using Simulation.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    class RemotePlayerSimulation : PlayerSimulation, IRemotePlayerSimulation
    {
        protected override void Awake()
        {
            base.Awake();
            PlayerMovePrediction = gameObject.AddComponent<RemotePlayerMovementPredictionSimulation>();
        }

        public ISimulation<RemotePlayerMovementPrediction> PlayerMovePrediction { get; set; }
    }
}