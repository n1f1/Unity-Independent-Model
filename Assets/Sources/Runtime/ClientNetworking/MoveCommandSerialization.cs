using System;
using Model.Characters;
using Networking.ObjectsHashing;
using Networking.Replication.ObjectCreationReplication;
using Networking.Replication.Serialization;
using Networking.StreamIO;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

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
            outputStream.Write(inObject.Position);
            Console.WriteLine(inObject.Position);
            outputStream.Write(inObject.CreationTime.Ticks);
        }

        public MoveCommand Deserialize(IInputStream inputStream)
        {
            short movementInstanceID = inputStream.ReadInt16();
            Vector3 moveDelta = inputStream.ReadVector3();
            long timeTicks = inputStream.ReadInt64();
            CharacterMovement movement = HashedObjects.GetInstance<CharacterMovement>(movementInstanceID);
            Console.Write($"movement id: {movementInstanceID} for {movement.Position}");

            return new MoveCommand(movement, moveDelta, TimeSpan.FromTicks(timeTicks));
        }
    }
}