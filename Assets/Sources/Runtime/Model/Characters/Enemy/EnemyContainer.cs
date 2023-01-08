using System.Collections.Generic;

namespace Model.Characters.Enemy
{
    public class EnemyContainer : IUpdatable
    {
        private readonly LinkedList<Enemy> _enemies = new();
        private readonly Stack<Enemy> _deadEnemies = new();
        
        public bool HasDead => _deadEnemies.Count > 0;

        public void Add(Enemy enemy)
        {
            _enemies.AddLast(enemy);
        }

        public void UpdateTime(float deltaTime)
        {
            for (LinkedListNode<Enemy> node = _enemies.First; node != null; node = node.Next)
            {
                Enemy enemy = node.Value;
                
                enemy.UpdateTime(deltaTime);

                if (enemy.Dead)
                {
                    _deadEnemies.Push(enemy);
                    _enemies.Remove(node);
                }
            }
        }

        public IEnumerable<Enemy> GetDead()
        {
            for (int i = 0; i < _deadEnemies.Count; i++)
                yield return _deadEnemies.Pop();
            
            _deadEnemies.Clear();
        }
    }
}