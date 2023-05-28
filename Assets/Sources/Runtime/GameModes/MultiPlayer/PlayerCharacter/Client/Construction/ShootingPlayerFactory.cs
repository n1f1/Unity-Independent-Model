using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Common.Construction;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.Shooting.Shooter;
using Model.SpatialObject;
using Simulation.Infrastructure;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Construction
{
    internal class ShootingPlayerFactory : IPlayerWithViewFactory
    {
        private readonly IBulletFactory<IBullet> _bulletFactory;
        private readonly BulletsContainer _bulletsContainer;
        private readonly IObjectToSimulationMap _objectToSimulationMap;

        public ShootingPlayerFactory(IBulletFactory<IBullet> bulletFactory, BulletsContainer bulletsContainer,
            IObjectToSimulationMap objectToSimulation)
        {
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
            _objectToSimulationMap = objectToSimulation ?? throw new ArgumentNullException(nameof(objectToSimulation));
        }

        public Player Create(Vector3 position, IPlayerView playerView)
        {
            Transform playerTransform = new Transform(playerView.PositionView, position);
            
            DisablePlayerSimulationDeath disablePlayerSimulationDeath =
                new DisablePlayerSimulationDeath(_objectToSimulationMap);
            
            playerView.DeathView = new CompositeDeath(playerView.DeathView, disablePlayerSimulationDeath);
            
            DamageableShooter damageableShooter = new DamageableShooter();

            CharacterShooter characterShooter =
                DefaultPlayer.CreateCharacterShooter(playerView, playerTransform, _bulletFactory, _bulletsContainer,
                    damageableShooter, Player.ShootingCooldown);

            Player player = DefaultPlayer.Player(playerTransform, characterShooter, playerView, damageableShooter);
            damageableShooter.Exclude(player.Damageable);
            disablePlayerSimulationDeath.SetPlayer(player);

            return player;
        }
    }
}