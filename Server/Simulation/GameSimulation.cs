using System;
using System.Collections.Generic;
using GameModes.MultiPlayer;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using Model.Shooting.Bullets;
using Networking.Server;
using Server.Simulation.CommandSend;
using Server.Simulation.Physics;
using GameClient = Server.Client.GameClient;

namespace Server.Simulation
{
    internal class GameSimulation
    {
        private readonly List<GameClient> _gameClients = new();
        private readonly IPhysicsSimulation _physicsSimulation;
        private readonly Room _room;
        private readonly BulletsContainer _bulletsContainer;
        private SimulationCommandSender<TakeDamageCommand> _commandSender;

        public GameSimulation(IPhysicsSimulation physicsSimulation, Room room, BulletsContainer bulletsContainer,
            SimulationCommandSender<TakeDamageCommand> commandSender)
        {
            _commandSender = commandSender ?? throw new ArgumentNullException(nameof(commandSender));
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
        }

        public IEnumerable<GameClient> GameClients => _gameClients;

        public void Add(GameClient player)
        {
            _gameClients.Add(player);
        }

        public void AddPassedTime(float time)
        {
            for (int i = 0; i < 5; i++)
            {
                _bulletsContainer.Update(time / 5f);
                _physicsSimulation.Update(time / 5f);
            }

            foreach (GameClient gameClient in _gameClients)
            {
                gameClient.Player.UpdateTime(time);
                gameClient.ProcessReceivedCommands();
                gameClient.SendOutgoingCommands(_room);
            }
            
            _commandSender.Send(_room);
        }
    }
}