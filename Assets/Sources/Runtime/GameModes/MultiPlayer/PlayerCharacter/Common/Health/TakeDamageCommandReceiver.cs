using Networking.Common.PacketReceive;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Health
{
    public class TakeDamageCommandReceiver : IReplicatedObjectReceiver<TakeDamageCommand>
    {
        public void Receive(TakeDamageCommand createdObject)
        {
            createdObject.Execute();
        }
    }
}