using System;
using System.Numerics;
using Model.Characters;
using Networking.ObjectsHashing;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;

namespace ClientNetworking
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
            outputStream.Write(inObject.MoveDelta);
        }

        public MoveCommand Deserialize(IInputStream inputStream)
        {
            short movementInstanceID = inputStream.ReadInt16();
            CharacterMovement movement = HashedObjects.GetInstance<CharacterMovement>(movementInstanceID);
            Vector3 moveDelta = inputStream.ReadVector3();
            Console.Write($"movement id: {movementInstanceID} for {movement.Position}");

            return new MoveCommand(movement, moveDelta);
        }
    }
}