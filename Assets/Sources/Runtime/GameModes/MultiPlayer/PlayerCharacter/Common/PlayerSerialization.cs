using System;
using Model.Characters;
using Networking.ObjectsHashing;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;
using Networking.StreamIO;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class PlayerSerialization : DefaultSerialization, ISerialization<Player>, IDeserialization<Player>
    {
        private readonly IPlayerFactory _playerFactory;

        public PlayerSerialization(IHashedObjectsList hashedObjects, ITypeIdConversion typeId,
            IPlayerFactory playerFactory) : base(hashedObjects, typeId)
        {
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
        }

        public void Serialize(Player player, IOutputStream outputStream)
        {
            CharacterMovement movement = player.CharacterMovement;
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(player));
            outputStream.Write(movement.Position);
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(movement));
        }

        public Player Deserialize(IInputStream inputStream)
        {
            short playerInstanceId = inputStream.ReadInt16();
            Vector3 position = inputStream.ReadVector3();
            short movementInstanceID = inputStream.ReadInt16();

            Debug.Log(position);
            var player = _playerFactory.CreatePlayer(position);

            HashedObjects.RegisterWithID(player, playerInstanceId);
            HashedObjects.RegisterWithID(player.CharacterMovement, movementInstanceID);
            Console.Write($" id: {playerInstanceId} position: {player.CharacterMovement.Position}");

            return player;
        }
    }
}