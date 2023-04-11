using GameModes.SinglePlayer;
using UnityEngine;
using Utility;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    public class RemotePlayerTemplate : SinglePlayerTemplate
    {
        [SerializeField] private MonoBehaviour _remotePlayerBehavior;

        protected override void OnValidate()
        {
            base.OnValidate();
            InspectorInterfaceInjection.TrySetObject<IRemotePlayerSimulation>(ref _remotePlayerBehavior);
        }

        private void Awake()
        {
            RemotePlayerSimulation = (IRemotePlayerSimulation) _remotePlayerBehavior;
        }

        public IRemotePlayerSimulation RemotePlayerSimulation { get; set; }
    }
}