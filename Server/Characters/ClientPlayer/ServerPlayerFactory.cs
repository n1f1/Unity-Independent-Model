using System;
using System.Numerics;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Server.Characters.Shooting;
using Server.Simulation;
using Server.Simulation.Physics;

namespace Server.Characters.ClientPlayer
{
    internal class ServerPlayerFactory : IPlayerFactory
    {
        private readonly BulletsContainer _bulletsContainer;
        private readonly IPhysicsSimulation _physicsSimulation;

        public ServerPlayerFactory(BulletsContainer bulletsContainer, IPhysicsSimulation physicsSimulation)
        {
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
            _bulletsContainer = bulletsContainer ?? throw new ArgumentNullException(nameof(bulletsContainer));
        }

        public Player CreatePlayer(Vector3 position)
        {
            IRigidbody rigidbody = _physicsSimulation.CreateCapsuleRigidbody(0.5f);
            rigidbody.UpdatePosition(position);
            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);
            Transform transform = new Transform(new UpdateRigidbody(rigidbody), position);

            DamageableShooter damageableShooter = new DamageableShooter();
            
            CharacterShooter characterShooter = new CharacterShooter(
                new ForwardAim(new NullAimView()),
                new DefaultGun(new ServerBulletFactory(_physicsSimulation), cooldown, _bulletsContainer, damageableShooter),
                transform);

            IDamageable damageable = new Health(Player.MAXHealth, new NullHealthView(),
                new Death(new NullDeathView()));

            var player = new Player(transform, damageable, characterShooter);
            damageableShooter.Exclude(player.Damageable);
            _physicsSimulation.RegisterCollidable(rigidbody, player);

            return player;
        }
    }
}