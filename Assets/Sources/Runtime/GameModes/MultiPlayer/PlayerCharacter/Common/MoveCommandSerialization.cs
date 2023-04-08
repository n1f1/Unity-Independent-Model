using System;
using Model.Characters;
using Networking.ObjectsHashing;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;
using Networking.StreamIO;
using Vector3 = System.Numerics.Vector3;

namespace GameModes.MultiPlayer.PlayerCharacter.Common
{
    public class MoveCommandSerialization : DefaultSerialization, ISerialization<MoveCommand>,
        IDeserialization<MoveCommand>
    {
        public MoveCommandSerialization(IHashedObjectsList hashedObjects, ITypeIdConversion typeId) : base(
            hashedObjects, typeId)
        {
        }

        public void Serialize(MoveCommand inObject, IOutputStream outputStream)
        {
            short movementId = HashedObjects.GetID(inObject.Movement);
            outputStream.Write(movementId);
            outputStream.Write(inObject.Acceleration);
            outputStream.Write(inObject.DeltaTime);
            outputStream.Write(inObject.Position);
            outputStream.Write(inObject.ID);
        }

        public MoveCommand Deserialize(IInputStream inputStream)
        {
            short movementInstanceID = inputStream.ReadInt16();
            Vector3 moveDelta = inputStream.ReadVector3();
            float deltaTime = inputStream.ReadSingle();
            Vector3 position = inputStream.ReadVector3();
            short id = inputStream.ReadInt16();
            CharacterMovement movement = HashedObjects.GetInstance<CharacterMovement>(movementInstanceID);
            Console.Write($"movement id: {movementInstanceID} for {movement.Position}");

            return new MoveCommand(movement, moveDelta, deltaTime, position, id);
        }
    }
}