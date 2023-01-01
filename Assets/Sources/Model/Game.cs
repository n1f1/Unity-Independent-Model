using System.Collections.Generic;
using System.Numerics;

namespace Model
{
    public class Game
    {
        private readonly IViewSimulationFactory _playerViewSimulationFactory;
        private readonly IPositionView _cameraPositionView;
        
        private Player _player;
        private List<IPositionView> _positionViews = new List<IPositionView>();
        private ISimulation _movementSimulation;

        public Game(IViewSimulationFactory playerViewSimulationFactory, IPositionView positionView)
        {
            _cameraPositionView = positionView;
            _playerViewSimulationFactory = playerViewSimulationFactory;
        }

        public void Start()
        {
            IViewSimulation playerSimulation = _playerViewSimulationFactory.Create();
            IPositionView positionView = playerSimulation.AddView<IPositionView>();
            _player = new Player(new CompositePositionView(positionView, _cameraPositionView));
            _movementSimulation = playerSimulation.AddSimulation(_player.CharacterMovement);
            _positionViews.Add(positionView);
        }

        public void Update(float deltaTime)
        {
            _movementSimulation.UpdatePassedTime(deltaTime);
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