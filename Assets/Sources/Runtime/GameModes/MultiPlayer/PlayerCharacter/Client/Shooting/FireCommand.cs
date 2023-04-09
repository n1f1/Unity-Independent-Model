using System;
using System.Numerics;
using Model.Characters;
using Model.Characters.Shooting.Bullets;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Shooting
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
            Player.CharacterShooter.Aim(PlayerPosition, AimPosition);

            if (Player.CharacterShooter.CanShoot() == false)
                return;

            Succeeded = true;
            Bullet = Player.CharacterShooter.Shoot();
        }
    }
}