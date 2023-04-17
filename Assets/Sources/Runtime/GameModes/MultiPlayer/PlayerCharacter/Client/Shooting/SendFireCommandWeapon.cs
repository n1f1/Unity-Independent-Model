using System;
using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.Player;
using Model.Shooting;
using Model.Shooting.Bullets;
using Networking.Common.PacketSend.ObjectSend;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Shooting
{
    internal class SendFireCommandWeapon : IWeapon
    {
        private readonly NotReconciledCommands<FireCommand> _reconciledCommands;
        private readonly INetworkObjectSender _sender;
        private readonly IWeapon _weapon;
        private readonly Player _player;
        private short _id;

        public SendFireCommandWeapon(Player player, INetworkObjectSender sender,
            NotReconciledCommands<FireCommand> reconciledCommands)
        {
            _reconciledCommands = reconciledCommands ?? throw new ArgumentNullException(nameof(reconciledCommands));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _weapon = _player.CharacterShooter.Weapon;
        }

        public IBullet Shoot(IAim aim)
        {
            FireCommand fireCommand =
                new FireCommand(_player, _player.Transform.Position, aim.Trajectory.Finish, ++_id);
            
            fireCommand.Execute();
            _sender.Send(fireCommand);
            _reconciledCommands.Add(fireCommand);

            return fireCommand.Bullet;
        }

        public bool CanShoot(IAim aim) =>
            _weapon.CanShoot(aim);

        public void CoolDown(float deltaTime) =>
            _weapon.CoolDown(deltaTime);
    }
}