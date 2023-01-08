using System;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Enemy;
using Model.SpatialObject;
using Simulation.Common;
using SimulationObject;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace ObjectComposition
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly SimulatedSimulationPool<Enemy> _objectPool = new(16);
        private readonly ISimulationProvider<Enemy> _simulationProvider;
        private readonly Player _player;

        public EnemyFactory(Player player, EnemySimulationProvider enemySimulationProvider)
        {
            _player = player ?? throw new ArgumentNullException();
            _simulationProvider = enemySimulationProvider ?? throw new ArgumentNullException();
        }

        public Enemy Create(Vector3 position)
        {
            if (!_objectPool.CanGet())
                AddNewToObjectPool(position);

            SimulatedSimulationPool<Enemy>.SimulatedPair simulatedPair = _objectPool.Get();
            Enemy enemy = CreateNewEnemy(position, simulatedPair.Poolable);
            _objectPool.Replace(simulatedPair.TObject, enemy);

            return enemy;
        }

        public void Destroy(Enemy enemy)
        {
            _objectPool.Return(enemy);
        }

        private void AddNewToObjectPool(Vector3 position)
        {
            SimulationObject<Enemy> simulation = _simulationProvider.CreateSimulationObject();
            Enemy enemy = CreateNewEnemy(position, simulation);
            _objectPool.AddNew(enemy, simulation);
        }

        private Enemy CreateNewEnemy(Vector3 position, SimulationObject<Enemy> simulation)
        {
            IDeath death = new Death();
            Health health = new Health(100f, simulation.GetView<IHealthView>(), death);
            Transform transform = new Transform(simulation.GetView<IPositionView>(), position);

            Enemy enemy = new Enemy(transform, death, health, _player);

            _simulationProvider.InitializeSimulation(simulation, enemy);
        
            return enemy;
        }
    }
}