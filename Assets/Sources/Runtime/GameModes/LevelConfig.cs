using GameModes.MultiPlayer.PlayerCharacter;
using GameModes.SinglePlayer.ObjectComposition.EnemyConstruction;
using GameModes.SinglePlayer.ObjectComposition.PlayerConstruction;
using UnityEngine;

namespace GameModes
{
    [CreateAssetMenu(menuName = "Create LevelConfigsList", fileName = "LevelConfigsList", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private SinglePlayerTemplate _playerTemplate;
        [SerializeField] private EnemyTemplate _enemyTemplate;
        [SerializeField] private GameObject _bulletTemplate;
        [SerializeField] private RemotePlayerTemplate _remotePlayerTemplate;

        public SinglePlayerTemplate PlayerTemplate => _playerTemplate;
        public EnemyTemplate EnemyTemplate => _enemyTemplate;
        public GameObject BulletTemplate => _bulletTemplate;
        public RemotePlayerTemplate RemotePlayerTemplate => _remotePlayerTemplate;
    }
}