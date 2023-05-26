using GameModes.MultiPlayer.PlayerCharacter.Client.Reconciliation;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.Player;
using Model.Shooting;
using Networking.Common.PacketSend.ObjectSend;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Shooting
{
    internal class FireCommandSender : ICharacterShooter
    {
        private readonly ICharacterShooter _characterShooter;
        private short _id;

        public FireCommandSender(Player player, INetworkObjectSender networkObjectSender,
            NotReconciledCommands<FireCommand> notReconciledFireCommands)
        {
            _characterShooter = player.CharacterCharacterShooter;
            Weapon = new SendFireCommandWeapon(player, networkObjectSender, notReconciledFireCommands);
            Aim = player.CharacterCharacterShooter.Aim;
        }

        public IWeapon Weapon { get; }
        public IAim Aim { get; }

        public void AimAt(Vector3 aimPosition) =>
            _characterShooter.AimAt(aimPosition);
    }
}