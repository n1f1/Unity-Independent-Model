using Networking;
using Networking.PacketReceive;

namespace GameModes.MultiPlayer
{
    public class CommandsReceiver : IReplicatedObjectReceiver<ICommand>
    {
        public void Receive(ICommand createdObject)
        {
            createdObject.Execute();
        }
    }
}