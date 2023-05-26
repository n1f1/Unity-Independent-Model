using GameModes.MultiPlayer.PlayerCharacter.Client.Construction;
using Model.Characters.CharacterHealth;
using Model.Shooting.Shooter;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Shooting
{
    internal class ExcludeFakeDamageableView : IShooter
    {
        public bool CanHit(IDamageable damageable) => 
            damageable is DamageableFakeView == false;
    }
}