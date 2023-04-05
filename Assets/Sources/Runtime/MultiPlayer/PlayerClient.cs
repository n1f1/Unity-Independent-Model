using Model.Characters;
using SimulationObject;

namespace MultiPlayer
{
    public class PlayerClient
    {
        private SimulationObject<Player> _playerSimulation;
        public Player Player { get; private set; }

        public void SetClientPlayerSimulation(Player player, SimulationObject<Player> playerSimulation)
        {
            Player = player;
            _playerSimulation = playerSimulation;
        }

        public void UpdateTime(float deltaTime)
        {
            _playerSimulation?.UpdateTime(deltaTime);
        }
    }
}