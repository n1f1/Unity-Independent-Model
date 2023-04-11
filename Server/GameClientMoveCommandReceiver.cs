using System;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using Networking.PacketReceive;

namespace Server
{
    internal class GameClientMoveCommandReceiver: IReplicatedObjectReceiver<MoveCommand>
    {
        private readonly PlayerToClientMap _playerToClientMap;

        public GameClientMoveCommandReceiver(PlayerToClientMap playerToClientMap)
        {
            _playerToClientMap = playerToClientMap ?? throw new ArgumentNullException(nameof(playerToClientMap));
        }

        public void Receive(MoveCommand command)
        {
            GameClient gameClient = _playerToClientMap.Get(command.Player);
            gameClient.AddCommand(command);
        }
    }
}