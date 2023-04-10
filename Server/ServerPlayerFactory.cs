using System;
using System.Numerics;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Model.SpatialObject;

namespace Server
{
    internal class ServerPlayerFactory : IPlayerFactory
    {
        private readonly BulletsContainer _bulletsContainer;
        private GameSimulation _gameSimulation;

        public ServerPlayerFactory(BulletsContainer bulletsContainer, GameSimulation gameSimulation)
        {
            _gameSimulation = gameSimulation ?? throw new ArgumentNullException(nameof(gameSimulation));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
        }
        
        public Player CreatePlayer(Vector3 position)
        {
            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);

            IPositionView positionView = new NullPositionVew();
            return new Player(new NullHealthView(), new ForwardAim(new NullAimView()),
                new NullDeathView(), new DefaultGun(new ServerBulletFactory(), cooldown, _bulletsContainer), cooldown, new Transform(positionView, position));
        }
    }
}