using System;
using Model.Characters;
using Model.Characters.Enemy;
using Simulation;
using Simulation.Pool;
using Object = UnityEngine.Object;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.SinglePlayer.ObjectComposition.EnemyConstruction
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly KeyPooledObjectPool<Enemy, PoolableSimulationObject<EnemyTemplate>> _objectPool = new();
        private readonly Player _player;
        private readonly EnemyTemplate _enemyTemplate;

        private PoolableSimulationObject<EnemyTemplate> _freeSimulationToReuse;

        public EnemyFactory(Player player, EnemyTemplate enemyTemplate)
        {
            _enemyTemplate = enemyTemplate ?? throw new ArgumentNullException(nameof(enemyTemplate));
            _player = player ?? throw new ArgumentNullException();
        }

        public Enemy Create(Vector3 position)
        {
            if (!_objectPool.CanGet())
                AddNewToObjectPool(position);

            return _objectPool.GetFreeByKey();
        }

        public void Destroy(Enemy enemy) =>
            _freeSimulationToReuse = _objectPool.RemoveActive(enemy);

        private void AddNewToObjectPool(Vector3 position)
        {
            PoolableSimulationObject<EnemyTemplate> simulation = GetSimulation();
            IEnemyView enemyView = simulation.Template.EnemyView;
            IEnemySimulation enemySimulation = simulation.Template.EnemySimulation;

            Enemy enemy = CreateNewEnemy(position, simulation, enemyView, enemySimulation);
            _objectPool.AddNew(enemy, simulation);
        }

        private Enemy CreateNewEnemy(Vector3 position, PoolableSimulationObject<EnemyTemplate> simulationObject,
            IEnemyView enemyView, IEnemySimulation simulation)
        {
            Enemy enemy = new Enemy(position, _player, enemyView.HealthView, enemyView.PositionView);
            simulationObject.AddUpdatableSimulation(simulation.Damageable.Initialize(enemy.Health));

            return enemy;
        }

        private PoolableSimulationObject<EnemyTemplate> GetSimulation()
        {
            PoolableSimulationObject<EnemyTemplate> simulation;

            if (_freeSimulationToReuse == null)
            {
                EnemyTemplate enemyTemplate = Object.Instantiate(_enemyTemplate);
                simulation = new PoolableSimulationObject<EnemyTemplate>(enemyTemplate);
            }
            else
            {
                simulation = _freeSimulationToReuse;
                simulation.Clear();
                _freeSimulationToReuse = null;
            }

            return simulation;
        }
    }
}