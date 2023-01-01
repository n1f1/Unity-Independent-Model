using System.Collections.Generic;
using System.Numerics;

namespace Model
{
    public class Game
    {
        private Player _player;
        private IViewSimulationFactory<Player> _playerViewSimulationFactory;
        private List<IPositionView> _positionViews = new List<IPositionView>();
        private ISimulation _movementSimulation;

        public Game(IViewSimulationFactory<Player> playerViewSimulationFactory)
        {
            _playerViewSimulationFactory = playerViewSimulationFactory;
        }

        public void Start()
        {
            IViewSimulation playerSimulation = _playerViewSimulationFactory.Create();
            IPositionView positionView = playerSimulation.AddView<IPositionView>();
            _player = new Player(positionView);
            _movementSimulation = playerSimulation.AddSimulation(_player.CharacterMovement);
            _positionViews.Add(positionView);
        }

        public void Update(float deltaTime)
        {
            _movementSimulation.UpdatePassedTime(deltaTime);
        }
    }

    public interface IInputSimulation
    {
    }

    public interface IPositionView : IView
    {
        void UpdatePosition(Vector3 position);
    }

    public interface IViewSimulationFactory<in TObject>
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