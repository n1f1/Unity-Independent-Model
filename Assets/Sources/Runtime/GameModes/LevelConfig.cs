using UnityEngine;

namespace GameModes
{
    [CreateAssetMenu(menuName = "Create LevelConfigsList", fileName = "LevelConfigsList", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private GameObject _playerTemplate;
        [SerializeField] private GameObject _enemyTemplate;
        [SerializeField] private GameObject _bulletTemplate;

        public GameObject PlayerTemplate => _playerTemplate;
        public GameObject EnemyTemplate => _enemyTemplate;
        public GameObject BulletTemplate => _bulletTemplate;
    }
}