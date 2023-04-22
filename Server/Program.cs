using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameModes.MultiPlayer;
using GameModes.MultiPlayer.PlayerCharacter.Client;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
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
using Server.Simulation.CommandSend;
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

            IGenericInterfaceList serialization =
                new GenericInterfaceWithParameterList(new Dictionary<Type, object>(), typeof(ISerialization<>));
            IGenericInterfaceList deserialization =
                new GenericInterfaceWithParameterList(new Dictionary<Type, object>(), typeof(IDeserialization<>));
            GenericInterfaceWithParameterList receivers = new GenericInterfaceWithParameterList(
                new Dictionary<Type, object>(), typeof(IReplicatedObjectReceiver<>));

            ObjectReplicationPacketFactory replicationPacketFactory =
                new ObjectReplicationPacketFactory(serialization, typeIdConversion);

            SimulationCommandSender<TakeDamageCommand> commandSender =
                new SimulationCommandSender<TakeDamageCommand>(replicationPacketFactory);

            IPhysicsSimulation physicsSimulation = new PhysicsSimulation();
            BulletsContainer bulletsContainer = new BulletsContainer(new PhysicsBulletDestroyer(physicsSimulation));
            GameSimulation game = new GameSimulation(physicsSimulation, room, bulletsContainer, commandSender);

            PlayerToClientMap playerToClientMap = new PlayerToClientMap();
            
            ServerPlayerFactory playerFactory =
                new ServerPlayerFactory(bulletsContainer, physicsSimulation, commandSender);

            PlayerSerialization playerSerialization =
                new PlayerSerialization(hashedObjects, playerFactory);

            serialization.Register(typeof(Player), playerSerialization);
            serialization.Register(typeof(ClientPlayer), new ClientPlayerSerialization(playerSerialization));
            serialization.Register(typeof(MoveCommand), new MoveCommandSerialization(hashedObjects));
            serialization.Register(typeof(FireCommand), new FireCommandSerialization(hashedObjects));
            serialization.Register(typeof(TakeDamageCommand), new TakeDamageCommandSerialization(hashedObjects));

            deserialization.Register(typeof(Player), playerSerialization);
            deserialization.Register(typeof(MoveCommand), new MoveCommandSerialization(hashedObjects));
            deserialization.Register(typeof(FireCommand), new FireCommandSerialization(hashedObjects));

            receivers.Register(typeof(MoveCommand), new GameClientMoveCommandReceiver(playerToClientMap));
            receivers.Register(typeof(FireCommand), new GameClientFireCommandReceiver(playerToClientMap));

            IReplicationPacketRead replicationPacketRead = new ReplicationPacketRead(new CreationReplicator(
                typeIdConversion, deserialization,
                new ReceivedReplicatedObjectMatcher(receivers)));

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