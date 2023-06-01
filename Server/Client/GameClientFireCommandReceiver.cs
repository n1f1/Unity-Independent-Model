using System;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Networking.Common.PacketReceive;

namespace Server.Client
{
    internal class GameClientFireCommandReceiver : IReplicatedObjectReceiver<FireCommand>
    {
        private readonly PlayerToClientMap _playerToClientMap;

        public GameClientFireCommandReceiver(PlayerToClientMap playerToClientMap)
        {
            _playerToClientMap = playerToClientMap ?? throw new ArgumentNullException(nameof(playerToClientMap));
        }

        public void Receive(FireCommand command)
        {
            GameClient gameClient = _playerToClientMap.Get(command.Player);
            gameClient.FireCommandHandler.AddCommand(command);
        }
    }
}