using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading.Tasks;
using ClientNetworking;
using ClientNetworking.NetworkingTypesConfigurations;
using Model.Characters;
using Networking;
using Networking.ObjectsHashing;
using Networking.Packets;
using Networking.PacketSender;
using Networking.Replication;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using ObjectComposition;
using Simulation.Common;

namespace Server
{
    class Program
    {
        private static int _id;
        private static Room _room = new();
        private static ObjectReplicationPacketFactory _replicationPacketFactory;
        private static List<Player> _players = new();

        static async Task Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 55555);
            tcpListener.Start();

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
            _replicationPacketFactory = new ObjectReplicationPacketFactory(dictionary, typeIdConversion);

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {
                    typeof(MoveCommand), new CompositeReceiver<ICommand>(
                        new CommandsReceiver(),
                        new CommandsResendReceiver(_room, _replicationPacketFactory))
                }
            };

            Dictionary<Type, IDeserialization<object>> deserializations =
                new Dictionary<Type, IDeserialization<object>>();

            deserializations.PopulateDictionary(deserialization);

            Replicator replicator = new Replicator(new CreationReplicator(typeIdConversion, deserializations,
                new ReceivedReplicatedObjectMatcher(receivers)));

            while (true)
            {
                ListenNewClients(tcpListener);

                foreach (Client client in _room.Clients)
                {
                    if (client.Disconnected == false)
                        ReceivePackets(client.InputStream, replicator);
                }
            }
        }

        private static void ListenNewClients(TcpListener tcpListener)
        {
            if (tcpListener.Pending())
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                tcpClient.NoDelay = true;
                tcpClient.SendBufferSize = 1500;
                Console.WriteLine("DemoGameClient connected...");
                Client client = new Client(tcpClient, ++_id);
                client.Welcome();
                _room.Add(client);
                AddClientToGame(client);
            }
        }

        private static void AddClientToGame(Client client)
        {
            Player player = new NullPlayerFactory().CreatePlayer(new Vector3(1, 0, 1));

            INetworkPacket networkPacket = _replicationPacketFactory.Create(player);

            foreach (Client each in _room.Clients)
                each.Sender.SendPacket(networkPacket);

            foreach (Player other in _players)
                client.Sender.SendPacket(_replicationPacketFactory.Create(other));

            _players.Add(player);
        }

        private static void ReceivePackets(IInputStream inputStream, Replicator replicator)
        {
            if (inputStream.NotEmpty() == false)
                return;

            PacketType packetType = (PacketType) inputStream.ReadInt32();
            Console.WriteLine("\nReceived " + packetType + " time: " + DateTime.Now.TimeOfDay);

            if (packetType == PacketType.ReplicationData)
                replicator.ProcessReplicationPacket(inputStream);
            else
                throw new InvalidOperationException();
        }
    }
}