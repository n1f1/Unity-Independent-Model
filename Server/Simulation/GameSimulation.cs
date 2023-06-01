using System;
using System.Collections.Generic;
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
        private const int _physicsIterationsPerFrame = 5;
        private const float _physicsIterationTime = (float) 1 / _physicsIterationsPerFrame;

        private readonly SimulationCommandSender<TakeDamageCommand> _commandSender;
        private readonly List<GameClient> _gameClients = new();
        private readonly IPhysicsSimulation _physicsSimulation;
        private readonly Room _room;
        private readonly BulletsContainer _bulletsContainer;

        public GameSimulation(IPhysicsSimulation physicsSimulation, Room room, BulletsContainer bulletsContainer,
            SimulationCommandSender<TakeDamageCommand> commandSender)
        {
            _commandSender = commandSender ?? throw new ArgumentNullException(nameof(commandSender));
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
        }

        public IEnumerable<GameClient> GameClients => _gameClients;

        public void Add(GameClient player) =>
            _gameClients.Add(player);

        public void AddPassedTime(float time)
        {
            for (int i = 0; i < _physicsIterationsPerFrame; i++)
            {
                _bulletsContainer.Update(time * _physicsIterationTime);
                _physicsSimulation.Update(time * _physicsIterationTime);
            }

            UpdateClients(time);
            _commandSender.Send(_room);
            RemoveDeadPlayers();
        }

        private void UpdateClients(float time)
        {
            foreach (GameClient gameClient in _gameClients)
            {
                gameClient.Player.UpdateTime(time);
                gameClient.ProcessReceivedCommands();
                gameClient.SendOutgoingCommands(_room);
            }
        }

        private void RemoveDeadPlayers()
        {
            for (var i = 0; i < _gameClients.Count; i++)
            {
                GameClient gameClient = _gameClients[i];

                if (gameClient.Player.Health.Dead)
                {
                    _gameClients.Remove(gameClient);
                    _room.Remove(gameClient.ServerClient);
                    --i;
                }
            }
        }
    }
}