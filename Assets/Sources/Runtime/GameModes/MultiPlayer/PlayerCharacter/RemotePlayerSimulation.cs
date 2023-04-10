using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer.ObjectComposition;
using Model;

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