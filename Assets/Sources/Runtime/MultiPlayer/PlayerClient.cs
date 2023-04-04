using Model.Characters;
using SimulationObject;

namespace MultiPlayer
{
    public class PlayerClient
    {
        private SimulationObject<Player> _playerSimulation;

        public void SetClientPlayerSimulation(SimulationObject<Player> playerSimulation)
        {
            _playerSimulation = playerSimulation;
        }

        public void UpdateTime(float deltaTime)
        {
            _playerSimulation?.UpdateTime(deltaTime);
        }
    }
}