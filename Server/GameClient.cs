using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using Model.Characters;
using Networking.Connection;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketSend;

namespace Server
{
    internal class GameClient
    {
        private readonly Queue<MoveCommand> _moveCommands = new();
        private readonly List<FireCommand> _fireCommands = new();
        private bool _processedMovement;
        private bool _processedFire;
        private Vector3 _acceleration;
        private float _deltaTime;
        private short _id;
        private ObjectReplicationPacketFactory _objectReplicationPacketFactory;

        public GameClient(Player player, ObjectReplicationPacketFactory objectReplicationPacketFactory)
        {
            _objectReplicationPacketFactory = objectReplicationPacketFactory;
            Player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public Player Player { get; }

        public void AddCommand(MoveCommand command) =>
            _moveCommands.Enqueue(command);

        public void AddCommand(FireCommand command) =>
            _fireCommands.Add(command);

        public void ProcessReceivedCommands()
        {
            _processedMovement = false;
            _processedFire = false;
            _deltaTime = 0;

            while (_moveCommands.TryDequeue(out MoveCommand command))
            {
                command.Execute();
                _acceleration = command.Acceleration;
                _deltaTime += command.DeltaTime;

                if (command.ID > _id)
                    _id = command.ID;

                _processedMovement = true;
            }

            foreach (FireCommand fireCommand in _fireCommands)
            {
                fireCommand.Execute();
                Console.WriteLine($"fire {fireCommand.Succeeded}");
                _processedFire = true;
            }
        }

        public void SendOutgoingCommands(Room room)
        {
            if (_processedMovement)
            {
                MoveCommand moveCommand =
                    new MoveCommand(Player, _acceleration, _deltaTime, Player.Transform.Position, _id);

                INetworkPacket movePacket = _objectReplicationPacketFactory.Create(moveCommand);

                Send(movePacket, room);
                _processedMovement = false;
            }

            if (_processedFire)
            {
                foreach (FireCommand fireCommand in _fireCommands)
                {
                    if (fireCommand.Succeeded)
                        Send(_objectReplicationPacketFactory.Create(fireCommand), room);
                }

                _fireCommands.Clear();
                _processedFire = false;
            }
        }

        public void Send(INetworkPacket networkPacket, Room room)
        {
            foreach (Client roomClient in room.Clients)
                roomClient.Sender.SendPacket(networkPacket);
        }
    }
}