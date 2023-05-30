using System;
using Model.Characters;
using Model.Characters.Player;
using Networking.Common.Replication.ObjectsHashing;
using Networking.Common.Replication.Serialization;
using Networking.Common.StreamIO;
using Networking.Common.Utilities;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class PlayerSerialization : DefaultSerialization, ISerialization<Player>, IDeserialization<Player>
    {
        private readonly IPlayerFactory _playerFactory;

        public PlayerSerialization(IHashedObjectsList hashedObjects, IPlayerFactory playerFactory) : base(hashedObjects)
        {
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
        }

        public void Serialize(Player player, IOutputStream outputStream)
        {
            CharacterMovement movement = player.CharacterMovement;
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(player));
            outputStream.Write(movement.Position);
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(movement));
            short registerOrGetRegistered = HashedObjects.RegisterOrGetRegistered(player.Damageable);
            outputStream.Write(registerOrGetRegistered);
            outputStream.Write(player.Health.Amount);
            Console.WriteLine(player.Health.Amount);
        }

        public Player Deserialize(IInputStream inputStream)
        {
            short playerInstanceId = inputStream.ReadInt16();
            Vector3 position = inputStream.ReadVector3();
            short movementInstanceID = inputStream.ReadInt16();
            short damageableInstanceID = inputStream.ReadInt16();
            float health = inputStream.ReadSingle();
            Player player = _playerFactory.CreatePlayer(new PlayerData(position, health));

            HashedObjects.RegisterWithID(player, playerInstanceId);
            HashedObjects.RegisterWithID(player.CharacterMovement, movementInstanceID);
            HashedObjects.RegisterWithID(player.Damageable, damageableInstanceID);

            return player;
        }
    }
}