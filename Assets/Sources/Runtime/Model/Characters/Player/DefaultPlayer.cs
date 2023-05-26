using Model.Characters.CharacterHealth;
using Model.Shooting;
using Model.Shooting.Bullets;
using Model.Shooting.Shooter;
using Model.SpatialObject;
using Server.Characters.ClientPlayer;
using static Model.Characters.Player.Player;

namespace Model.Characters.Player
{
    public static class DefaultPlayer
    {
        public static CharacterShooter CreateCharacterShooter(IPlayerView playerView, Transform transform,
            IBulletFactory<IBullet> bulletFactory, BulletsContainer bulletsContainer, IShooter shooter,
            float shootingCooldown)
        {
            CharacterShooter characterShooter = new CharacterShooter(
                new ForwardAim(playerView.ForwardAimView),
                new DefaultGun(bulletFactory, new Cooldown(shootingCooldown), bulletsContainer, shooter),
                transform);

            return characterShooter;
        }

        public static Player Player(Transform transform, CharacterShooter characterShooter, IPlayerView playerView,
            DamageableShooter shooter)
        {
            Health damageable = new Health(MAXHealth, new NullHealthView(), new Death(playerView.DeathView));
            Player player = new Player(transform, damageable, damageable, characterShooter, shooter);

            return player;
        }
    }
}