using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Characters;

namespace ClientNetworking.NetworkingTypesConfigurations
{
    public static class SerializableTypesIdMap
    {
        public static IEnumerable<(Type, int)> Get()
        {
            return StringToInt(Create());
        }

        public static IEnumerable<(Type, string)> Create()
        {
            IEnumerable<(Type, string)> tuples = new List<(Type, string)>
            {
                (typeof(Player), "PLYR"),
                (typeof(MoveCommand), "CMVE")
            };

            return tuples;
        }

        public static IEnumerable<(Type, int)> StringToInt(IEnumerable<(Type, string)> input)
        {
            return input.Select(tuple => (tuple.Item1, BitConverter.ToInt32(Encoding.UTF8.GetBytes(tuple.Item2))));
        }
    }
}