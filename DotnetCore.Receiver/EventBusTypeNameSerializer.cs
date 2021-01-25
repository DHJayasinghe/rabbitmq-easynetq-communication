using EasyNetQ;
using System;
using System.Collections.Concurrent;

namespace DotnetCore.Receiver
{
    public sealed class EventBusTypeNameSerializer : ITypeNameSerializer
    {
        private readonly ConcurrentDictionary<Type, string> serializedTypes = new ConcurrentDictionary<Type, string>();
        private readonly ConcurrentDictionary<string, Type> deSerializedTypes = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Define our own de-serialization rule
        /// </summary>
        public Type DeSerialize(string typeName)
        {
            return deSerializedTypes.GetOrAdd(typeName, t =>
            {
                var type = Type.GetType(typeName);
                if (type == null)
                {
                    throw new EasyNetQException("Cannot find type {0}", t);
                }
                return type;
            });
        }

        /// <summary>
        /// Define our own serialization rule
        /// </summary>
        public string Serialize(Type type)
        {
            return serializedTypes.GetOrAdd(type, t =>
            {
                // Take only the type full name instead of default qualified full assembly name
                var typeName = type.FullName;
                if (typeName.Length > 255)
                {
                    throw new EasyNetQException("The serialized name of type '{0}' exceeds the AMQP " +
                                                  "maximum short string length of 255 characters.", t.Name);
                }
                return typeName;
            });
        }
    }
}
