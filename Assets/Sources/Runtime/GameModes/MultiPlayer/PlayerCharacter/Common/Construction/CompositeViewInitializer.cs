using System;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Construction
{
    internal class CompositeViewInitializer<TView> : IViewInitializer<TView>
    {
        private readonly IViewInitializer<TView>[] _initializers;

        public CompositeViewInitializer(params IViewInitializer<TView>[] initializers)
        {
            _initializers = initializers ?? throw new ArgumentNullException(nameof(initializers));
        }

        public void InitializeView(TView playerView)
        {
            foreach (IViewInitializer<TView> initializer in _initializers) 
                initializer.InitializeView(playerView);
        }
    }
}