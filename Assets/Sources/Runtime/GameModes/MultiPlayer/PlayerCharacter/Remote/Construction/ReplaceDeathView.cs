using System;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using Model.Characters.CharacterHealth;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    internal class ReplaceDeathView : IViewInitializer<ICharacterWithHealthView>
    {
        private readonly IDeathView _deathView;

        public ReplaceDeathView(IDeathView deathView)
        {
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
        }

        public void InitializeView(ICharacterWithHealthView playerView)
        {
            playerView.DeathView = _deathView;
        }
    }
}