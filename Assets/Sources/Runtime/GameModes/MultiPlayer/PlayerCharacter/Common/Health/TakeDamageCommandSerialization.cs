using Model.Characters.CharacterHealth;
using Networking.Common.Replication.ObjectsHashing;
using Networking.Common.Replication.Serialization;
using Networking.Common.StreamIO;

namespace GameModes.MultiPlayer.PlayerCharacter.Common.Health
{
    public class TakeDamageCommandSerialization : DefaultSerialization, ISerialization<TakeDamageCommand>,
        IDeserialization<TakeDamageCommand>
    {
        public TakeDamageCommandSerialization(IHashedObjectsList hashedObjects) : base(hashedObjects)
        {
        }

        public void Serialize(TakeDamageCommand inObject, IOutputStream outputStream)
        {
            outputStream.Write(HashedObjects.RegisterOrGetRegistered(inObject.Damageable));
            outputStream.Write(inObject.Damage);
        }

        public TakeDamageCommand Deserialize(IInputStream inputStream)
        {
            short playerID = inputStream.ReadInt16();
            float damage = inputStream.ReadSingle();
            IDamageable damageable = HashedObjects.GetInstance<IDamageable>(playerID);

            return new TakeDamageCommand(damageable, damage);
        }
    }
}