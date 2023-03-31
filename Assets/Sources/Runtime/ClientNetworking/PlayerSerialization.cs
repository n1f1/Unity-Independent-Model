using System;
using Model.Characters;
using Model.SpatialObject;
using Networking.ObjectsHashing;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using ObjectComposition;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace ClientNetworking
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
            outputStream.Write(HashedObjects.Register(player));
            outputStream.Write(movement.Position);
            outputStream.Write(HashedObjects.Register(movement));
        }

        public Player Deserialize(IInputStream inputStream)
        {
            short playerInstanceId = inputStream.ReadInt16();
            Vector3 position = inputStream.ReadVector3();
            short movementInstanceID = inputStream.ReadInt16();

            Debug.Log(position);
            Player player = _playerFactory.CreatePlayer(position);

            HashedObjects.RegisterNew(player, playerInstanceId);
            HashedObjects.RegisterNew(player.CharacterMovement, movementInstanceID);
            Console.Write($" id: {playerInstanceId} position: {player.CharacterMovement.Position}");

            return player;
        }
    }
}