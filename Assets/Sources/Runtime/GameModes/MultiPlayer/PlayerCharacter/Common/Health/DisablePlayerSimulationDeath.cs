using System;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Simulation;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Health
{
    internal class DisablePlayerSimulationDeath : IDeathView
    {
        private readonly IObjectToSimulationMap _objectToSimulation;
        private Player _player;

        public DisablePlayerSimulationDeath(IObjectToSimulationMap objectToSimulation)
        {
            _objectToSimulation = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public void SetPlayer(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public void Die()
        {
            SimulationObject simulationObject = _objectToSimulation.Get(_player);
            simulationObject.Disable();
        }
    }
}