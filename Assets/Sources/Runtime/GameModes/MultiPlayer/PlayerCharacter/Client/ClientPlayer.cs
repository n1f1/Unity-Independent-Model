using Model.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
{
    public class ClientPlayer
    {
        public Player Player { get; }

        public ClientPlayer(Player player)
        {
            Player = player;
        }
    }
}