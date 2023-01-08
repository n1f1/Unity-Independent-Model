using System;
using System.Collections.Generic;
using System.Numerics;
using Model.SpatialObject;

namespace Model.Characters.Enemy
{
    public class EnemySpawner
    {
        private readonly EnemyContainer _enemyContainer;
        private readonly IEnemyFactory _enemyFactory;
        private readonly Transform _spawnCenter;
        private readonly int _enemyCount;

        public EnemySpawner(int enemyCount, EnemyContainer enemyContainer, IEnemyFactory enemyFactory,
            Transform spawnCenter)
        {
            _spawnCenter = spawnCenter;
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException();
            _enemyContainer = enemyContainer ?? throw new ArgumentNullException();

            if (enemyCount < 0)
                throw new ArgumentOutOfRangeException();

            _enemyCount = enemyCount;
        }

        public void Start()
        {
            Random random = new Random();

            for (int i = 0; i < _enemyCount; i++) 
                CreateEnemy(random);
        }

        public void Update()
        {
            if (_enemyContainer.HasDead)
                DestroyDeadEnemies(_enemyContainer.GetDead());
        }

        private void DestroyDeadEnemies(IEnumerable<Enemy> dead)
        {
            Random random = new Random();
            
            foreach (Enemy enemy in dead)
            {
                _enemyFactory.Destroy(enemy);
                CreateEnemy(random);
            }
        }

        private void CreateEnemy(Random random)
        {
            Vector3 position = new Vector3(GetOutOfScreenAxisPosition(random), 0, GetAxisPosition(random));
            Enemy enemy = _enemyFactory.Create(_spawnCenter.Position + position);
            _enemyContainer.Add(enemy);
        }

        private float GetAxisPosition(Random random) => 
            random.Next(-30, 30);

        private int GetOutOfScreenAxisPosition(Random random)
        {
            int axis = random.Next(0, 2);
            axis = Math.Sign(axis - 0.5f) * random.Next(30, 35);
            
            return axis;
        }
    }
}