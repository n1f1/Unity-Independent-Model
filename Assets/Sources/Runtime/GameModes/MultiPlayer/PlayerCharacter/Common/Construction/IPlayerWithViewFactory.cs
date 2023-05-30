using Model.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal interface IPlayerWithViewFactory
    {
        Player Create(PlayerData playerData, IPlayerView playerView);
    }
}