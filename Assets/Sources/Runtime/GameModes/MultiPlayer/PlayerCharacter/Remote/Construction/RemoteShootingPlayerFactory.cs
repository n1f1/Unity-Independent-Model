using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Shooting.Bullets;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    internal class RemoteShootingPlayerFactory : IPlayerWithViewFactory
    {
        private readonly BulletsContainer _bulletsContainer;
        private readonly IBulletFactory<IBullet> _bulletFactory;

        public RemoteShootingPlayerFactory(PooledBulletFactory bulletFactory, BulletsContainer bulletsContainer)
        {
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
        }

        public Player Create(Vector3 position, IPlayerView playerView)
        {
            Transform playerTransform = new Transform(playerView.PositionView, position);

            playerView.ForwardAimView = new NullAimView();
            IBulletFactory<IBullet> bulletFactory = new RemoteFiredBulletFactory(playerTransform, _bulletFactory);

            DamageableShooter damageableShooter = new DamageableShooter();

            CharacterShooter characterShooter =
                DefaultPlayer.CreateCharacterShooter(playerView, playerTransform, bulletFactory, _bulletsContainer,
                    damageableShooter, 0f);

            Player player = DefaultPlayer.Player(playerTransform, characterShooter, playerView, damageableShooter);
            damageableShooter.Exclude(player.Damageable);

            return player;
        }
    }
}