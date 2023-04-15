using System;
using Model.Characters.CharacterHealth;
using Model.SpatialObject;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal class AddPositionView : IViewInitializer<ICharacterWithHealthView>
    {
        private readonly IPositionView _cameraView;

        public AddPositionView(IPositionView cameraView)
        {
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
        }

        public void InitializeView(ICharacterWithHealthView playerView)
        {
            playerView.PositionView = new CompositePositionView(playerView.PositionView, _cameraView);
        }
    }
}