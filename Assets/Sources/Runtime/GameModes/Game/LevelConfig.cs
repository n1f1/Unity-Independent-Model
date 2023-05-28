using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer;
using Simulation.Characters.EnemyCharacter;
using Simulation.Shooting.Bullets;
using UnityEngine;

namespace GameModes.Game
{
    [CreateAssetMenu(menuName = "Create LevelConfigsList", fileName = "LevelConfigsList", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private SinglePlayerTemplate _playerTemplate;
        [SerializeField] private EnemyTemplate _enemyTemplate;
        [SerializeField] private BulletTemplate _bulletTemplate;
        [SerializeField] private RemotePlayerTemplate _remotePlayerTemplate;
        [SerializeField] private SinglePlayerTemplate _clientPlayerTemplate;

        public SinglePlayerTemplate PlayerTemplate => _playerTemplate;
        public EnemyTemplate EnemyTemplate => _enemyTemplate;
        public BulletTemplate BulletTemplate => _bulletTemplate;
        public RemotePlayerTemplate RemotePlayerTemplate => _remotePlayerTemplate;
        public SinglePlayerTemplate ClientPlayerTemplate => _clientPlayerTemplate;
    }
}