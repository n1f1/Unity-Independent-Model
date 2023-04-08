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
    public class PlayerSerialization : DefaultSerialization, ISerialization<Model.Characters.Player>, IDeserialization<Model.Characters.Player>
    {
        private readonly IPlayerFactory _playerFactory;

        public PlayerSerialization(IHashedObjectsList hashedObjects, ITypeIdConversion typeId,
            IPlayerFactory playerFactory) : base(hashedObjects, typeId)
        {
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
        }

        public void Serialize(Model.Characters.Player player, IOutputStream outputStream)
        {
            CharacterMovement movement = player.CharacterMovement;
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(player));
            outputStream.Write(movement.Position);
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(movement));
        }

        public Model.Characters.Player Deserialize(IInputStream inputStream)
        {
            short playerInstanceId = inputStream.ReadInt16();
            Vector3 position = inputStream.ReadVector3();
            short movementInstanceID = inputStream.ReadInt16();

            Debug.Log(position);
            Model.Characters.Player player = _playerFactory.CreatePlayer(position);

            HashedObjects.RegisterWithID(player, playerInstanceId);
            HashedObjects.RegisterWithID(player.CharacterMovement, movementInstanceID);
            Console.Write($" id: {playerInstanceId} position: {player.CharacterMovement.Position}");

            return player;
        }
    }
}