using Model.Characters;
using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
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
            Player?.UpdateTime(deltaTime);
            _playerSimulation?.UpdateTime(deltaTime);
        }
    }
}