using System;
using System.Numerics;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using Model.Characters.Player;
using Model.Shooting.Bullets;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Shooting
{
    public class FireCommand : ICommand, IReconciliationCommand
    {
        public FireCommand(Player player, Vector3 transformPosition, Vector3 aimPosition, short id)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            PlayerPosition = transformPosition;
            AimPosition = aimPosition;
            ID = id;
        }

        public Vector3 PlayerPosition { get; }
        public Vector3 AimPosition { get; }
        public Player Player { get; }
        public short ID { get; }
        public bool Succeeded { get; private set; }
        public IBullet Bullet { get; private set; }

        public void Execute()
        {
            CharacterShooter shooter = Player.CharacterShooter;
            shooter.Aim.Aim(PlayerPosition, AimPosition);

            if (shooter.Weapon.CanShoot(shooter.Aim) == false)
                return;

            Succeeded = true;
            Bullet = shooter.Weapon.Shoot(shooter.Aim);
        }
    }
}