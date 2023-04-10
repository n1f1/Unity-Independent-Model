using System;
using System.Collections.Generic;
using System.Linq;
using Model.Characters;
using Model.Characters.Shooting.Bullets;
using Networking.Connection;

namespace Server
{
    internal class GameSimulation
    {
        private readonly List<GameClient> _gameClients = new();
        private Room _room;
        private BulletsContainer _bulletsContainer;
        public IEnumerable<GameClient> GameClients => _gameClients;

        public void Add(GameClient player, Room room)
        {
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _gameClients.Add(player);
        }

        public void AddPassedTime(float time)
        {
            Console.WriteLine(time);
            for (int i = 0; i < 5; i++)
            {
                _bulletsContainer.Update(time / 5f);
                
                foreach (GameClient gameClient in _gameClients)
                {
                    foreach (PhysicBullet bullet in gameClient.FiredBullets)
                    {
                        IEnumerable<Player> otherPlayers = _gameClients.Where(client => client != gameClient)
                            .Select(client => client.Player);

                        bullet.ProcessCollisions(otherPlayers);
                    }

                    gameClient.RemoveCollidedBullets();
                }
            }

            foreach (GameClient gameClient in _gameClients)
            {
                gameClient.Player.UpdateTime(time);
                gameClient.ProcessReceivedCommands();
                gameClient.SendOutgoingCommands(_room);
            }
        }

        public void Add(BulletsContainer bulletsContainer)
        {
            _bulletsContainer = bulletsContainer;
        }
    }
}