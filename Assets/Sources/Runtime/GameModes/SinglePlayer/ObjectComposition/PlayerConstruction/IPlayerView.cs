using GameModes.SinglePlayer.ObjectComposition.CharacterWithHealth;
using Model.Characters.Shooting;

namespace GameModes.SinglePlayer.ObjectComposition.PlayerConstruction
{
    public interface IPlayerView : ICharacterWithHealthView
    {
        IForwardAimView ForwardAimView { get; set; }
    }
}