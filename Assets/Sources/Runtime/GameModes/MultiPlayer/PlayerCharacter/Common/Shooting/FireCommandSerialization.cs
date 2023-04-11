using System.Numerics;
using Model.Characters.Player;
using Networking.ObjectsHashing;
using Networking.PacketReceive.Replication.ObjectCreationReplication;
using Networking.PacketReceive.Replication.Serialization;
using Networking.StreamIO;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Shooting
{
    public class FireCommandSerialization : DefaultSerialization, ISerialization<FireCommand>, IDeserialization<FireCommand>
    {
        public FireCommandSerialization(IHashedObjectsList hashedObjects, ITypeIdConversion typeIdConversion) : base(
            hashedObjects, typeIdConversion)
        {
        }

        public void Serialize(FireCommand inObject, IOutputStream outputStream)
        {
            outputStream.Write(HashedObjects.GetID(inObject.Player));
            outputStream.Write(inObject.PlayerPosition);
            outputStream.Write(inObject.AimPosition);
            outputStream.Write(inObject.ID);
        }

        public FireCommand Deserialize(IInputStream inputStream)
        {
            short playerID = inputStream.ReadInt16();
            Vector3 playerPosition = inputStream.ReadVector3();
            Vector3 aimPosition = inputStream.ReadVector3();
            short commandID = inputStream.ReadInt16();

            Player player = HashedObjects.GetInstance<Player>(playerID);
            return new FireCommand(player, playerPosition, aimPosition, commandID);
        }
    }
}