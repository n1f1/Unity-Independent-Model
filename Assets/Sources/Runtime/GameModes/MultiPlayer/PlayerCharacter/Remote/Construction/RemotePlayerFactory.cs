using System;
using GameModes.MultiPlayer.PlayerCharacter.Remote.Shooting;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting.Bullets;
using Simulation;
using Object = UnityEngine.Object;
using Transform = Model.SpatialObject.Transform;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Remote.Construction
{
    internal class RemotePlayerFactory : IPlayerFactory
    {
        private readonly RemotePlayerSimulationInitializer _simulationInitializer;
        private readonly IDeathView _deathView;
        private IBulletFactory<IBullet> _bulletFactory;
        private readonly BulletsContainer _bulletsContainer;
        private readonly RemotePlayerTemplate _playerTemplate;

        public RemotePlayerFactory(RemotePlayerTemplate playerTemplate,
            IBulletFactory<IBullet> pooledBulletFactory,
            IDeathView deathView, BulletsContainer bulletsContainer,
            RemotePlayerSimulationInitializer simulationInitializer)
        {
            _simulationInitializer =
                simulationInitializer ?? throw new ArgumentNullException(nameof(simulationInitializer));
            _playerTemplate = playerTemplate;
            _deathView = deathView ?? throw new ArgumentNullException(nameof(deathView));
            _bulletFactory = pooledBulletFactory;
            _bulletsContainer = bulletsContainer;
        }

        public Player CreatePlayer(Vector3 position)
        {
            RemotePlayerTemplate playerTemplate = Object.Instantiate(_playerTemplate);
            SimulationObject simulation = new SimulationObject(playerTemplate.gameObject);
            IPlayerView playerView = playerTemplate.PlayerView;
            IRemotePlayerSimulation playerSimulation = playerTemplate.RemotePlayerSimulation;
            playerView.DeathView = _deathView;

            Transform playerTransform = new Transform(playerView.PositionView, position);
            
            _bulletFactory = new RemoteFiredBulletFactory(playerTransform, _bulletFactory);

            CharacterShooter characterShooter =
                DefaultPlayer.CreateCharacterShooter(playerView, playerTransform, _bulletFactory, _bulletsContainer);
            
            Player player = DefaultPlayer.Player(playerTransform, characterShooter, playerView);

            _simulationInitializer.InitializeSimulation(player, playerSimulation, simulation);
            
            return player;
        }
    }
}