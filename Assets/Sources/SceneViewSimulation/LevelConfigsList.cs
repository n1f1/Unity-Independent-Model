using UnityEngine;

namespace SceneViewSimulation
{
    [CreateAssetMenu(menuName = "Create LevelConfigsList", fileName = "LevelConfigsList", order = 0)]
    public class LevelConfigsList : ScriptableObject
    {
        [SerializeField] private GameObject _playerTemplate;
        [SerializeField] private GameObject _enemyTemplate;

        public GameObject PlayerTemplate => _playerTemplate;
        public GameObject EnemyTemplate => _enemyTemplate;
    }
}