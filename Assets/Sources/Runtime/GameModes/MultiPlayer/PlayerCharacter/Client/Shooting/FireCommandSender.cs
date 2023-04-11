using System;
using Model.Characters;
using Model.Characters.Player;
using Model.Characters.Shooting;
using Model.Characters.Shooting.Bullets;
using Networking.PacketSend.ObjectSend;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Shooting
{
    internal class FireCommandSender : ICharacterShooter
    {
        private readonly Player _player;
        private readonly INetworkObjectSender _networkObjectSender;
        private readonly ICharacterShooter _characterShooter;
        private readonly NotReconciledCommands<FireCommand> _notReconciledFireCommands;
        private Vector3 _aimPosition;
        private short _id;

        public FireCommandSender(Player player, INetworkObjectSender networkObjectSender,
            NotReconciledCommands<FireCommand> notReconciledFireCommands)
        {
            _notReconciledFireCommands = notReconciledFireCommands;
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _characterShooter = player.CharacterShooter;
            _networkObjectSender = networkObjectSender ?? throw new ArgumentNullException(nameof(networkObjectSender));
        }

        public void AimAt(Vector3 aimPosition)
        {
            _aimPosition = aimPosition;
            _characterShooter.AimAt(aimPosition);
        }

        public IBullet Shoot()
        {
            FireCommand fireCommand = new FireCommand(_player, _player.Transform.Position, _aimPosition, ++_id);
            _networkObjectSender.Send(fireCommand);
            _notReconciledFireCommands.Add(fireCommand);

            return _characterShooter.Shoot();
        }

        public void StopAiming() =>
            _characterShooter.StopAiming();

        public bool CanShoot() =>
            _player.Weapon.CanShoot(_player.Aim);
    }
}