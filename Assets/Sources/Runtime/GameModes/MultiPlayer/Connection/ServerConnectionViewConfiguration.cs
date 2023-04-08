using UnityEngine;

namespace GameModes.MultiPlayer.Connection
{
    [CreateAssetMenu(menuName = "Create ServerConnectionViewConfiguration", fileName = "ServerConnectionViewConfiguration", order = 0)]
    public class ServerConnectionViewConfiguration : ScriptableObject
    {
        [SerializeField] private ServerConnectionWindow _connectionWindow;

        public ServerConnectionWindow ConnectionWindow => _connectionWindow;
    }
}