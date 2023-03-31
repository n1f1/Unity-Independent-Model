using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClientNetworking;
using ClientNetworking.NetworkingTypesConfigurations;
using Model.Characters;
using Networking;
using Networking.ObjectsHashing;
using Networking.Replication;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;

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

            IPacketReceiver packetReceiver = new PacketReceiver(replicationPacketRead);

            while (true)
            {
                newClientsListener.ListenNewClients(tcpListener);

                foreach (Client client in room.Clients)
                {
                    if (client.IsConnected)
                        packetReceiver.ReceivePackets(client.InputStream, replicationPacketRead);
                }
            }
        }
    }
}