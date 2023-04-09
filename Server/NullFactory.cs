using System.Numerics;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;

namespace Server
{
    internal class ServerPlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(Vector3 position)
        {
            BulletsContainer bulletsContainer = new BulletsContainer(new NullBulletDestroyer());
            Cooldown cooldown = new Cooldown(Player.ShootingCooldown);

            return new Player(new NullPositionVew(), new NullHealthView(), new ForwardAim(new NullAimView()), position,
                new NullDeathView(), new DefaultGun(new NullBulletFactory(), cooldown, bulletsContainer),
                bulletsContainer, cooldown);
        }
    }
}