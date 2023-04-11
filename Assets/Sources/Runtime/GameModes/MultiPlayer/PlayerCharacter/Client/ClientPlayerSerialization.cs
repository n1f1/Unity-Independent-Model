using System;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using Networking.PacketReceive.Replication.Serialization;
using Networking.StreamIO;

namespace GameModes.MultiPlayer.PlayerCharacter.Client
{
    public class ClientPlayerSerialization : ISerialization<ClientPlayer>, IDeserialization<ClientPlayer>
    {
        private readonly PlayerSerialization _playerSerialization;

        public ClientPlayerSerialization(PlayerSerialization playerSerialization)
        {
            _playerSerialization = playerSerialization ?? throw new ArgumentNullException(nameof(playerSerialization));
        }

        public void Serialize(ClientPlayer inObject, IOutputStream outputStream) => 
            _playerSerialization.Serialize(inObject.Player, outputStream);

        public ClientPlayer Deserialize(IInputStream inputStream) => 
            new(_playerSerialization.Deserialize(inputStream));
    }
}