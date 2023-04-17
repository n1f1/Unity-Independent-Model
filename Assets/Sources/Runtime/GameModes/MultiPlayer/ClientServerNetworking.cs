using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.Client;
using Networking.Client.Connection;
using Networking.Common;
using Networking.Common.PacketReceive;
using Networking.Common.PacketSend.ObjectSend;
using Networking.Common.Replication;
using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Common.Replication.Serialization;
using Networking.Common.StreamIO;

namespace GameModes.MultiPlayer
{
    public class ClientServerNetworking : IServerConnection
    {
        private readonly ServerConnection _serverConnection;
        private readonly ITypeIdConversion _typeIdConversion;

        private IInputStream _inputStream;
        private GameClient _gameClient;
        private INetworkStreamRead _streamRead;

        public ClientServerNetworking(IServerConnectionView connectionView, ITypeIdConversion typeIdConversion)
        {
            _serverConnection = new ServerConnection(connectionView);
            _typeIdConversion = typeIdConversion;
        }

        public INetworkObjectSender ObjectSender { get; private set; }
        public IGenericInterfaceList Deserialization { get; private set; }
        public IGenericInterfaceList Serialization { get; private set; }
        public IGenericInterfaceList Receivers { get; private set; }

        public INetworkStreamRead StreamRead
        {
            get => _streamRead;
            set => _streamRead = value ?? throw new ArgumentNullException(nameof(value));
        }

        public async Task<bool> Connect()
        {
            if (await _serverConnection.Connect() == false)
                return false;

            NetworkStream networkStream = _serverConnection.Client.GetStream();
            IOutputStream outputStream = new BinaryWriterOutputStream(networkStream);
            _inputStream = new BinaryReaderInputStream(networkStream);

            Deserialization =
                new GenericInterfaceWithParameterList(new Dictionary<Type, object>(), typeof(IDeserialization<>));

            Serialization =
                new GenericInterfaceWithParameterList(new Dictionary<Type, object>(), typeof(ISerialization<>));

            Receivers = new GenericInterfaceWithParameterList(new Dictionary<Type, object>(),
                typeof(IReplicatedObjectReceiver<>));

            ObjectSender = new StreamObjectSender(outputStream,
                new ObjectReplicationPacketFactory(Serialization, _typeIdConversion));

            IReplicationPacketRead packetRead =
                new SendToReceiversPacketRead(Receivers, Deserialization, _typeIdConversion);

            _gameClient = new GameClient(packetRead);
            StreamRead = _gameClient;

            return true;
        }

        public void ReadNetworkStream() => 
            StreamRead.ReadNetworkStream(_inputStream);
    }
}