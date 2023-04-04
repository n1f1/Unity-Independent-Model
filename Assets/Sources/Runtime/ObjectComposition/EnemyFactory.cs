using System;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using Simulation.Pool;
using SimulationObject;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace ObjectComposition
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly KeyPooledObjectPool<Enemy, SimulationObject<Enemy>> _objectPool = new();

        private readonly ISimulationProvider<Enemy> _simulationProvider;
        private readonly Player _player;
        private SimulationObject<Enemy> _freeSimulation;

        public EnemyFactory(Player player, EnemySimulationProvider enemySimulationProvider)
        {
            _player = player ?? throw new ArgumentNullException();
            _simulationProvider = enemySimulationProvider ?? throw new ArgumentNullException();
        }

        public Enemy Create(Vector3 position)
        {
            if (!_objectPool.CanGet())
                AddNewToObjectPool(position);

            return _objectPool.GetFreeByKey();
        }

        public void Destroy(Enemy enemy)
        {
            _freeSimulation = _objectPool.RemoveActive(enemy);
        }

        private void AddNewToObjectPool(Vector3 position)
        {
            SimulationObject<Enemy> simulation = _freeSimulation ?? _simulationProvider.CreateSimulationObject();
            _freeSimulation = null;
            Enemy enemy = CreateNewEnemy(position, simulation);
            _objectPool.AddNew(enemy, simulation);
        }

        private Enemy CreateNewEnemy(Vector3 position, SimulationObject<Enemy> simulation)
        {
            IDeath death = new Death(new NullDeathView());
            Health health = new Health(100f, simulation.GetView<IHealthView>(), death);
            Transform transform = new Transform(simulation.GetView<IPositionView>(), position);

            Enemy enemy = new Enemy(transform, death, health, _player);

            _simulationProvider.InitializeSimulation(simulation, enemy);

            return enemy;
        }
    }
}