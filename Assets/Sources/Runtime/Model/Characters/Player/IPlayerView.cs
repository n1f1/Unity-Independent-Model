using Model.Characters.CharacterHealth;
using Model.Shooting;

namespace Model.Characters.Player
{
    public interface IPlayerView : ICharacterWithHealthView
    {
        IForwardAimView ForwardAimView { get; set; }
    }
}