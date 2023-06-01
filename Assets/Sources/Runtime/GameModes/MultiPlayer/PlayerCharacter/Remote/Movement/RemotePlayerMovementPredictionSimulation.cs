using Simulation;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Movement
{
    internal class RemotePlayerMovementPredictionSimulation : MonoBehaviour, ISimulation<RemotePlayerMovementPrediction>
    {
        private RemotePlayerMovementPrediction _remotePlayerMovementPrediction;

        public ISimulation<RemotePlayerMovementPrediction> Initialize(RemotePlayerMovementPrediction prediction)
        {
            _remotePlayerMovementPrediction = prediction;
            return this;
        }

        public void UpdateTime(float deltaTime) =>
            _remotePlayerMovementPrediction.UpdateTime(deltaTime);
    }
}