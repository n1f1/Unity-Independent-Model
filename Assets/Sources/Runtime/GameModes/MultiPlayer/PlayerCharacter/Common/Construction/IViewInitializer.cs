namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal interface IViewInitializer<in TView>
    {
        void InitializeView(TView playerView);
    }
}