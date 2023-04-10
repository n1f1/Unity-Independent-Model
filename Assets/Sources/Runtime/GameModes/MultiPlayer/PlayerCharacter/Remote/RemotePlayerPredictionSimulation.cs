using Model;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    internal class RemotePlayerPredictionSimulation : MonoBehaviour, ISimulation<RemotePlayerPrediction>
    {
        private RemotePlayerPrediction _remotePlayerPrediction;

        public ISimulation<RemotePlayerPrediction> Initialize(RemotePlayerPrediction remotePlayerPrediction)
        {
            _remotePlayerPrediction = remotePlayerPrediction;
            return this;
        }

        public void UpdateTime(float deltaTime)
        {
            _remotePlayerPrediction.UpdateTime(deltaTime);
        }
    }
}