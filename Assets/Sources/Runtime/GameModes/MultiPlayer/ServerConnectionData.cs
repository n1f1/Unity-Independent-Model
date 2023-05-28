using UnityEngine;

namespace GameModes.MultiPlayer
{
    [CreateAssetMenu(menuName = "Create ServerConnectionData", fileName = "ServerConnectionData", order = 0)]
    public class ServerConnectionData : ScriptableObject
    {
        [SerializeField] private string _ip;
        [SerializeField] private int _port;

        public string IP => _ip;
        public int Port => _port;
    }
}