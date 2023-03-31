using System;
using System.Collections.Generic;
using Networking.Replication.ObjectCreationReplication;

namespace ClientNetworking
{
    public class TypeIdConversion : ITypeIdConversion
    {
        private readonly Dictionary<Type, int> _typeToIdHash;
        private readonly Dictionary<int, Type> _idToTypeHash;

        public TypeIdConversion(Dictionary<Type, int> typeToIdHash)
        {
            _typeToIdHash = typeToIdHash ?? throw new ArgumentNullException(nameof(typeToIdHash));

            _idToTypeHash = new Dictionary<int, Type>();

            foreach (KeyValuePair<Type, int> pair in _typeToIdHash)
                _idToTypeHash.Add(pair.Value, pair.Key);
        }

        public int GetTypeID<T>() => _typeToIdHash[typeof(T)];

        public Type GetTypeByID(int classId) => _idToTypeHash[classId];
    }
}