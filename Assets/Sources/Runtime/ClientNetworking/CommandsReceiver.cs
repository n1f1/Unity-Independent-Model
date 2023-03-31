using Networking;

namespace ClientNetworking
{
    public class CommandsReceiver : IReplicatedObjectReceiver<ICommand>
    {
        public void Receive(ICommand command)
        {
            command.Execute();
        }
    }
}