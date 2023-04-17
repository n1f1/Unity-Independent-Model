using Networking.Common.Replication.ObjectCreationReplication;
using Networking.Server;

namespace Server.Client
{
    internal interface ICommandHandler
    {
        bool Dirty { get; }
        void ProcessReceivedCommands();
        void SendOutgoingCommands(Room room, ObjectReplicationPacketFactory objectReplicationPacketFactory);
    }

    internal interface ICommandHandler<TCommand> : ICommandHandler
    {
        void AddCommand(TCommand command);
    }
}