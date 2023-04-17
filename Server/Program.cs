﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameModes.MultiPlayer;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Networking.Common;
using Networking.Common.PacketReceive;
using Networking.Common.Replication;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Common.Replication.ObjectsHashing;
using Networking.Common.Replication.Serialization;
using Networking.Server;
using Networking.Server.Connection;
using Server.Characters.ClientPlayer;
using Server.Characters.Shooting;
using Server.Client;
using Server.Simulation;
using Server.Simulation.Physics;
using Server.Update;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 55555);
            tcpListener.Start();
            Room room = new();

            IHashedObjectsList hashedObjects = new HashedObjectsList();
            ITypeIdConversion typeIdConversion = new TypeIdConversion(
                new Dictionary<Type, int>().PopulateDictionaryFromTuple(SerializableTypesIdMap.Get()));

            IPhysicsSimulation physicsSimulation = new PhysicsSimulation();
            BulletsContainer bulletsContainer = new BulletsContainer(new PhysicsBulletDestroyer(physicsSimulation));
            GameSimulation game = new GameSimulation(physicsSimulation, room, bulletsContainer);
            ServerPlayerFactory playerFactory = new ServerPlayerFactory(bulletsContainer, physicsSimulation);

            PlayerSerialization playerSerialization =
                new PlayerSerialization(hashedObjects, playerFactory);

            IEnumerable<(Type, object)> serialization = new List<(Type, object)>
            {
                (typeof(Player), playerSerialization),
                (typeof(ClientPlayer), new ClientPlayerSerialization(playerSerialization)),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects)),
                (typeof(FireCommand), new FireCommandSerialization(hashedObjects))
            };

            IEnumerable<(Type, object)> deserialization = new List<(Type, object)>
            {
                (typeof(Player), playerSerialization),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects)),
                (typeof(FireCommand), new FireCommandSerialization(hashedObjects))
            };

            Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
            dictionary.PopulateDictionary(serialization);

            ObjectReplicationPacketFactory replicationPacketFactory =
                new ObjectReplicationPacketFactory(
                    new GenericInterfaceWithParameterList(dictionary, typeof(ISerialization<>)), typeIdConversion);

            PlayerToClientMap playerToClientMap = new PlayerToClientMap();

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {typeof(MoveCommand), new GameClientMoveCommandReceiver(playerToClientMap)},
                {typeof(FireCommand), new GameClientFireCommandReceiver(playerToClientMap)}
            };

            Dictionary<Type, object> deserializations = new Dictionary<Type, object>();
            deserializations.PopulateDictionary(deserialization);

            IGenericInterfaceList receiver =
                new GenericInterfaceWithParameterList(receivers, typeof(IReplicatedObjectReceiver<>));
            
            IReplicationPacketRead replicationPacketRead = new ReplicationPacketRead(new CreationReplicator(
                typeIdConversion, new GenericInterfaceWithParameterList(deserializations, typeof(IDeserialization<>)),
                new ReceivedReplicatedObjectMatcher(receiver)));

            NewClientsListener newClientsListener = new NewClientsListener(room,
                new ClientConnectionPlayerCreation(replicationPacketFactory, room, game, playerToClientMap,
                    playerFactory));

            IPacketReceiver packetReceiver = new PacketReceiver();

            ServerUpdate update = new ServerUpdate(newClientsListener, replicationPacketRead, room, tcpListener,
                packetReceiver, game);

            FixedUpdateLoop fixedUpdateLoop = new FixedUpdateLoop(100, update);

            while (true)
            {
                fixedUpdateLoop.Update();
                Task.Yield();
            }
        }
    }
}