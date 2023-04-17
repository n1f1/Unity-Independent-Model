using System;
using System.Collections.Generic;
using Model.Shooting.Bullets;
using Networking.Connection;
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

        public GameSimulation(IPhysicsSimulation physicsSimulation, Room room, BulletsContainer bulletsContainer)
        {
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
        }
    }
}