using Model.Characters.Player;
using UnityEngine;
using Utility;

namespace GameModes.SinglePlayer.ObjectComposition.PlayerConstruction
{
    public class SinglePlayerTemplate : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _playerViewBehavior;
        [SerializeField] private MonoBehaviour _playerSimulationBehaviour;

        public IPlayerView PlayerViewBehavior => (IPlayerView) _playerViewBehavior;
        public IPlayerSimulation PlayerSimulationBehaviour => (IPlayerSimulation) _playerSimulationBehaviour;

        protected virtual void OnValidate()
        {
            InspectorInterfaceInjection.TrySetObject<IPlayerView>(ref _playerViewBehavior);
            InspectorInterfaceInjection.TrySetObject<IPlayerSimulation>(ref _playerSimulationBehaviour);
        }
    }
}