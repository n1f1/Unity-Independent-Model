using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer.ObjectComposition;
using GameModes.SinglePlayer.ObjectComposition.PlayerConstruction;
using Model;
using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter
{
    class RemotePlayerSimulation : PlayerSimulation, IRemotePlayerSimulation
    {
        protected override void Awake()
        {
            base.Awake();
            PlayerMovePrediction = gameObject.AddComponent<RemotePlayerPredictionSimulation>();
        }

        public ISimulation<RemotePlayerPrediction> PlayerMovePrediction { get; set; }
    }
}