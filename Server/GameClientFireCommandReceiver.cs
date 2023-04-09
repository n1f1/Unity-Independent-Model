using System;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using Networking.PacketReceive;

namespace Server
{
    internal class GameClientFireCommandReceiver: IReplicatedObjectReceiver<FireCommand>
    {
        private readonly PlayerToClientMap _playerToClientMap;

        public GameClientFireCommandReceiver(PlayerToClientMap playerToClientMap)
        {
            _playerToClientMap = playerToClientMap ?? throw new ArgumentNullException(nameof(playerToClientMap));
        }

        public void Receive(FireCommand command)
        {
            GameClient gameClient = _playerToClientMap.Get(command.Player);
            gameClient.AddCommand(command);
        }
    }
}