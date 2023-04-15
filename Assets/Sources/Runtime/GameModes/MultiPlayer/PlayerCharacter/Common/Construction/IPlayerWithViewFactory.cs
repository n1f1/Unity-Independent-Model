using System.Numerics;
using Model.Characters.Player;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal interface IPlayerWithViewFactory
    {
        Player Create(Vector3 position, IPlayerView playerView);
    }
}