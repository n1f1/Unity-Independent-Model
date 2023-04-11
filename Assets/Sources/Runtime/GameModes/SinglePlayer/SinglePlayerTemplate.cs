using Model.Characters.Player;
using Simulation.Characters.Player;
using UnityEngine;
using Utility;

namespace GameModes.SinglePlayer
{
    public class SinglePlayerTemplate : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _playerViewBehavior;
        [SerializeField] private MonoBehaviour _playerSimulationBehaviour;

        public IPlayerView PlayerView => (IPlayerView) _playerViewBehavior;
        public IPlayerSimulation PlayerSimulation => (IPlayerSimulation) _playerSimulationBehaviour;

        protected virtual void OnValidate()
        {
            InspectorInterfaceInjection.TrySetObject<IPlayerView>(ref _playerViewBehavior);
            InspectorInterfaceInjection.TrySetObject<IPlayerSimulation>(ref _playerSimulationBehaviour);
        }
    }
}