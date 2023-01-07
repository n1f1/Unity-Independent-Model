using System;
using System.Collections.Generic;
using System.Numerics;

namespace Model.Characters.Enemy
{
    public class EnemySpawner
    {
        private readonly int _enemyCount;
        private readonly IEnemyFactory _enemyFactory;
        private readonly EnemyContainer _enemyContainer;

        public EnemySpawner(int enemyCount, EnemyContainer enemyContainer, IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException();
            _enemyContainer = enemyContainer ?? throw new ArgumentNullException();

            if (enemyCount < 0)
                throw new ArgumentException();

            _enemyCount = enemyCount;
        }

        public void Start()
        {
            Random random = new Random();

            for (int i = 0; i < _enemyCount; i++)
            {
                Vector3 position = new Vector3(random.Next(-50, 50), 0, random.Next(-50, 50));
                Enemy enemy = _enemyFactory.Create(position);
                _enemyContainer.Add(enemy);
            }
        }

        public void Update()
        {
            if (_enemyContainer.HasDead)
                DestroyDeadEnemies(_enemyContainer.GetDead());
        }

        private void DestroyDeadEnemies(IEnumerable<Enemy> dead)
        {
            foreach (Enemy enemy in dead)
            {
                _enemyFactory.Destroy(enemy);
            }
        }
    }
}