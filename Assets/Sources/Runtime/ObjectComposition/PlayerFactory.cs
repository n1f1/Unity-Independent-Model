using System;
using System.Collections.Generic;
using ClientNetworking;
using Model;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;
using Simulation.Pool;
using SimulationObject;
using UnityEngine;
using View.Factories;
using Vector3 = System.Numerics.Vector3;

namespace ObjectComposition
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IViewFactory<IPositionView> _positionViewFactory;
        private readonly LevelConfig _levelConfig;
        private readonly IPositionView _cameraView;
        private readonly PlayerSimulationProvider _playerSimulationProvider;
        private readonly List<IUpdatable> _simulations = new();
        private readonly IObjectSender _objectSender;

        public PlayerFactory(LevelConfig levelConfig, IViewFactory<IPositionView> positionViewFactory,
            IViewFactory<IHealthView> healthViewFactory, IPositionView cameraView, IObjectSender objectSender)
        {
            _objectSender = objectSender ?? throw new ArgumentNullException(nameof(objectSender));
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _levelConfig = levelConfig ? levelConfig : throw new ArgumentNullException();
            _positionViewFactory = positionViewFactory ?? throw new ArgumentNullException();
            _playerSimulationProvider = new PlayerSimulationProvider(levelConfig.PlayerTemplate, positionViewFactory,
                healthViewFactory);
        }

        public Player CreatePlayer(Vector3 position)
        {
            var playerSimulation = _playerSimulationProvider.CreateSimulationObject();
            PooledBulletFactory bulletFactory = CreatePooledBulletFactory();

            IPositionView positionView = playerSimulation.GetView<IPositionView>();

            if (_simulations.Count == 0)
                positionView = new CompositePositionView(positionView, _cameraView);

            Player player = new Player(
                positionView,
                playerSimulation.GetView<IHealthView>(),
                new ForwardAim(playerSimulation.GetView<IForwardAimView>()), bulletFactory, bulletFactory, position);

            IMovable movable = player.CharacterMovement;
            
            if(Game.Multiplayer)
                movable = new MovementCommandSender(player.CharacterMovement, _objectSender);

            if (_simulations.Count == 0)
                _playerSimulationProvider.InitializeSimulation(playerSimulation, player, movable);

            playerSimulation.Enable();
            _simulations.Add(playerSimulation);
            _simulations.Add(player);

            return player;
        }

        private PooledBulletFactory CreatePooledBulletFactory()
        {
            GameObject bulletTemplate = _levelConfig.BulletTemplate;

            PooledBulletFactory bulletFactory =
                new PooledBulletFactory(
                    new KeyPooledObjectPool<DefaultBullet, SimulationObject<DefaultBullet>>(64),
                    new BulletSimulationProvider(bulletTemplate, _positionViewFactory));

            bulletFactory.PopulatePool();

            return bulletFactory;
        }

        public void Update(float deltaTime)
        {
            foreach (IUpdatable updatable in _simulations)
            {
                updatable.UpdateTime(deltaTime);
            }
        }
    }
}