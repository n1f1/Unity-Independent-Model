using System;
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

            GameSimulation game = new GameSimulation();
            BulletsContainer bulletsContainer = new BulletsContainer(new NullBulletDestroyer());
            ServerPlayerFactory playerFactory = new ServerPlayerFactory(bulletsContainer, game);
            game.Add(bulletsContainer);

            PlayerSerialization playerSerialization =
                new PlayerSerialization(hashedObjects, typeIdConversion, playerFactory);
            
            IEnumerable<(Type, object)> serialization = new List<(Type, object)>
            {
                (typeof(Player), playerSerialization),
                (typeof(ClientPlayer), new ClientPlayerSerialization(playerSerialization)),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion)),
                (typeof(FireCommand), new FireCommandSerialization(hashedObjects, typeIdConversion))
            };

            IEnumerable<(Type, object)> deserialization = new List<(Type, object)>
            {
                (typeof(Player), playerSerialization),
                (typeof(MoveCommand), new MoveCommandSerialization(hashedObjects, typeIdConversion)),
                (typeof(FireCommand), new FireCommandSerialization(hashedObjects, typeIdConversion))
            };

            Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
            dictionary.PopulateDictionary(serialization);
            ObjectReplicationPacketFactory replicationPacketFactory =
                new ObjectReplicationPacketFactory(dictionary, typeIdConversion);

            PlayerToClientMap playerToClientMap = new PlayerToClientMap();

            Dictionary<Type, object> receivers = new Dictionary<Type, object>
            {
                {typeof(MoveCommand), new GameClientMoveCommandReceiver(playerToClientMap)},
                {typeof(FireCommand), new GameClientFireCommandReceiver(playerToClientMap)}
            };

            Dictionary<Type, IDeserialization<object>> deserializations =
                new Dictionary<Type, IDeserialization<object>>();

            deserializations.PopulateDictionary(deserialization);

            IReplicationPacketRead replicationPacketRead = new ReplicationPacketRead(new CreationReplicator(
                typeIdConversion, deserializations, new ReceivedReplicatedObjectMatcher(receivers)));

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