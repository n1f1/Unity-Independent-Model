using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Model.SpatialObject;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class ShootingPlayerFactory : IPlayerWithViewFactory
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly BulletsContainer _bulletsContainer;

        public ShootingPlayerFactory(IBulletFactory<IBullet> bulletFactory, BulletsContainer bulletsContainer)
        {
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
        }

        public Player Create(Vector3 position, IPlayerView playerView)
        {
            Transform playerTransform = new Transform(playerView.PositionView, position);

            CharacterShooter characterShooter =
                DefaultPlayer.CreateCharacterShooter(playerView, playerTransform, _bulletFactory, _bulletsContainer);

            Player player = DefaultPlayer.Player(playerTransform, characterShooter, playerView);
            
            return player;
        }
    }
}