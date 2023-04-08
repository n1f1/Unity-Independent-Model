using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
{
    public class PlayerClient
    {
        private SimulationObject<Model.Characters.Player> _playerSimulation;
        public Model.Characters.Player Player { get; private set; }

        public void SetClientPlayerSimulation(Model.Characters.Player player, SimulationObject<Model.Characters.Player> playerSimulation)
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