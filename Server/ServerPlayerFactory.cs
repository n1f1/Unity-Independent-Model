using System;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;

namespace Server
{
    internal class ServerPlayerFactory : IPlayerFactory
    {
        private readonly BulletsContainer _bulletsContainer;

        public ServerPlayerFactory(BulletsContainer bulletsContainer)
        {
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
        }

        public Player CreatePlayer(Vector3 position)
        {
            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);
            Transform transform = new Transform(new NullPositionVew(), position);

            CharacterShooter characterShooter = new CharacterShooter(
                new ForwardAim(new NullAimView()),
                new DefaultGun(new ServerBulletFactory(), cooldown, _bulletsContainer),
                transform);

            IDamageable damageable = new Health(Player.MAXHealth, new NullHealthView(), new Death(new NullDeathView()));
            
            return new Player(transform, damageable, characterShooter);
        }
    }
}