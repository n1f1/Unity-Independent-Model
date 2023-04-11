using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameModes.MultiPlayer.PlayerCharacter.Client.Shooting;
using GameModes.MultiPlayer.PlayerCharacter.Common;
using GameModes.MultiPlayer.PlayerCharacter.Common.Movement;
using GameModes.MultiPlayer.PlayerCharacter.Common.Shooting;
using Model.Characters.Player;

namespace GameModes.MultiPlayer
{
    public static class SerializableTypesIdMap
    {
        public static IEnumerable<(Type, int)> Get()
        {
            return StringToInt(Create());
        }

        private static IEnumerable<(Type, string)> Create()
        {
            IEnumerable<(Type, string)> tuples = new List<(Type, string)>
            {
                (typeof(Player), "PLYR"),
                (typeof(MoveCommand), "CMVE"),
                (typeof(FireCommand), "FIRE")
            };

            return tuples;
        }

        private static IEnumerable<(Type, int)> StringToInt(IEnumerable<(Type, string)> input)
        {
            return input.Select(tuple => (tuple.Item1, BitConverter.ToInt32(Encoding.UTF8.GetBytes(tuple.Item2))));
        }
    }
}