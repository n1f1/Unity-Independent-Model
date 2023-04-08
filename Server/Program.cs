using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameModes.MultiPlayer;
using GameModes.MultiPlayer.NetworkingTypesConfigurations;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using Model.Characters;
using Networking.Connection;
using Networking.ObjectsHashing;
using Networking.PacketReceive;
using Networking.PacketReceive.Replication;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;

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

            IEnumerable<(Type, object)> serialization = new List<(Type, object)>
            {
                (typeof(Player), new PlayerSerialization(hashedObjects, typeIdConversion, new NullPlayerFactory())),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion))
            };

            IEnumerable<(Type, object)> deserialization = new List<(Type, object)>
            {
                (typeof(Player), new PlayerSerialization(hashedObjects, typeIdConversion, new NullPlayerFactory())),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion))
            };

            Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
            dictionary.PopulateDictionary(serialization);
            ObjectReplicationPacketFactory replicationPacketFactory =
                new ObjectReplicationPacketFactory(dictionary, typeIdConversion);

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {
                    typeof(MoveCommand), new CompositeReceiver<ICommand>(
                        new CommandsReceiver(),
                        new CommandsResendReceiver(room, replicationPacketFactory))
                }
            };

            Dictionary<Type, IDeserialization<object>> deserializations =
                new Dictionary<Type, IDeserialization<object>>();

            deserializations.PopulateDictionary(deserialization);

            IReplicationPacketRead replicationPacketRead = new ReplicationPacketRead(new CreationReplicator(
                typeIdConversion, deserializations, new ReceivedReplicatedObjectMatcher(receivers)));

            NewClientsListener newClientsListener = new NewClientsListener(room,
                new ClientConnectionPlayerCreation(replicationPacketFactory, room, new GameLevel()));

            IPacketReceiver packetReceiver = new PacketReceiver();

            ServerUpdate update = new ServerUpdate(newClientsListener, replicationPacketRead, room, tcpListener,
                packetReceiver);
            
            FixedUpdateLoop fixedUpdateLoop = new FixedUpdateLoop(100, update);

            while (true)
            {
                fixedUpdateLoop.Update();
                Task.Yield();
            }
        }
    }
}