using Model.Characters.Player;
using Networking.Common.PacketReceive;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote
{
    public class RemotePlayerReceiver : IReplicatedObjectReceiver<Player>
    {
        public void Receive(Player createdObject)
        {
        }
    }
}