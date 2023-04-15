using Model.Characters.Player;
using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    public class ClientPlayerSimulation
    {
        private SimulationObject _playerSimulation;
        public Player Player { get; private set; }

        public void SetClientPlayerSimulation(Player player, SimulationObject playerSimulation)
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