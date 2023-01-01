using System.Collections.Generic;
using System.Numerics;

namespace Model
{
    public class Game
    {
        private readonly IViewSimulationFactory _playerViewSimulationFactory;
        private readonly IViewSimulationFactory _enemyViewSimulationFactory;
        private readonly IPositionView _cameraPositionView;
        
        private Player _player;
        private List<IPositionView> _positionViews = new List<IPositionView>();
        private ISimulation _movementSimulation;
        private Enemy _enemy;

        public Game(IViewSimulationFactory playerViewSimulationFactory, IViewSimulationFactory enemyViewSimulationFactory, IPositionView positionView)
        {
            _enemyViewSimulationFactory = enemyViewSimulationFactory;
            _cameraPositionView = positionView;
            _playerViewSimulationFactory = playerViewSimulationFactory;
        }

        public void Start()
        {
            IViewSimulation playerSimulation = _playerViewSimulationFactory.Create();
            IPositionView positionView = playerSimulation.AddView<IPositionView>();
            IHealthView healthView = playerSimulation.AddView<IHealthView>();
            _player = new Player(new CompositePositionView(positionView, _cameraPositionView), healthView);
            _movementSimulation = playerSimulation.AddSimulation(_player.CharacterMovement);
            _positionViews.Add(positionView);

            IViewSimulation enemySimulation = _enemyViewSimulationFactory.Create();
            IPositionView enemyPositionView = enemySimulation.AddView<IPositionView>();
            _enemy = new Enemy(enemyPositionView, _player.Transform, _player.Health);
        }

        public void Update(float deltaTime)
        {
            _movementSimulation.UpdatePassedTime(deltaTime);
            _enemy.Update(deltaTime);
        }
    }

    public interface IPositionView : IView
    {
        void UpdatePosition(Vector3 position);
    }

    public interface IViewSimulationFactory
    {
        IViewSimulation Create();
    }

    public interface IViewSimulation
    {
        T AddView<T>() where T : IView;
        ISimulation AddSimulation<TSimulation>(TSimulation simulation);
    }

    public interface ISimulation : IUpdatable
    {
    }

    public interface IView
    {
    }
}