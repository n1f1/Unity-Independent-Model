using System;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.Player;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Server;

namespace Server.Client
{
    internal class GameClient
    {
        private readonly ObjectReplicationPacketFactory _objectReplicationPacketFactory;
        private readonly ICommandHandler[] _commandHandlers;

        public GameClient(Player player, ObjectReplicationPacketFactory objectReplicationPacketFactory,
            ServerClient serverClient)
        {
            _objectReplicationPacketFactory = objectReplicationPacketFactory;
            ServerClient = serverClient ?? throw new ArgumentNullException(nameof(serverClient));
            Player = player ?? throw new ArgumentNullException(nameof(player));
            MoveCommandHandler = new MoveCommandHandler(player);
            FireCommandHandler = new FireCommandHandler(player);
            _commandHandlers = new ICommandHandler[] {MoveCommandHandler, FireCommandHandler};
        }

        public ServerClient ServerClient { get; }
        public ICommandHandler<MoveCommand> MoveCommandHandler { get; }
        public ICommandHandler<FireCommand> FireCommandHandler { get; }
        public Player Player { get; }

        public void ProcessReceivedCommands()
        {
            foreach (ICommandHandler commandHandler in _commandHandlers)
            {
                if (commandHandler.Dirty)
                    commandHandler.ProcessReceivedCommands();
            }
        }

        public void SendOutgoingCommands(Room room)
        {
            foreach (ICommandHandler commandHandler in _commandHandlers)
            {
                if (commandHandler.Dirty)
                    commandHandler.SendOutgoingCommands(room, _objectReplicationPacketFactory);
            }
        }
    }
}