using Model.Characters.CharacterHealth;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.SpatialObject;
using static Model.Characters.Player.Player;

namespace Model.Characters.Player
{
    public static class DefaultPlayer
    {
        public static CharacterShooter CreateCharacterShooter(IPlayerView playerView, Transform transform,
            IBulletFactory<IBullet> bulletFactory, BulletsContainer bulletsContainer, IShooter shooter)
        {
            CharacterShooter characterShooter = new CharacterShooter(
                new ForwardAim(playerView.ForwardAimView),
                new DefaultGun(bulletFactory, new Cooldown(ShootingCooldown), bulletsContainer, shooter),
                transform);

            return characterShooter;
        }

        public static Player Player(Transform transform, CharacterShooter characterShooter, IPlayerView playerView)
        {
            Player player = new Player(
                transform,
                new Health(MAXHealth, playerView.HealthView, new Death(playerView.DeathView)),
                characterShooter);

            return player;
        }
    }
}