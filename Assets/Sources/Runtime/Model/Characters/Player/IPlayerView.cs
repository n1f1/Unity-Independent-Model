using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;

namespace Model.Characters.Player
{
    public interface IPlayerView : ICharacterWithHealthView
    {
        IForwardAimView ForwardAimView { get; set; }
    }
}