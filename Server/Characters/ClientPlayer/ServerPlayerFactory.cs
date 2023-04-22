using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using Server.Characters.Shooting;
using Server.Simulation.CommandSend;
using Server.Simulation.Physics;

namespace Server.Characters.ClientPlayer
{
    internal class ServerPlayerFactory : IPlayerFactory
    {
        private readonly BulletsContainer _bulletsContainer;
        private readonly IPhysicsSimulation _physicsSimulation;
        private readonly SimulationCommandSender<TakeDamageCommand> _commandHandler;

        public ServerPlayerFactory(BulletsContainer bulletsContainer, IPhysicsSimulation physicsSimulation,
            SimulationCommandSender<TakeDamageCommand> commandHandler)
        {
            _physicsSimulation = physicsSimulation ?? throw new ArgumentNullException(nameof(physicsSimulation));
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
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
                new DefaultGun(new ServerBulletFactory(_physicsSimulation), cooldown, _bulletsContainer,
                    damageableShooter),
                transform);

            Health health = new Health(Player.MAXHealth, new NullHealthView(), new Death(new NullDeathView()));
            IDamageable damageable = new TakeDamageCommandSender(_commandHandler, health);

            Player player = new Player(transform, damageable, characterShooter);
            damageableShooter.Exclude(player.Damageable);
            _physicsSimulation.RegisterCollidable(rigidbody, player);

            return player;
        }
    }
}