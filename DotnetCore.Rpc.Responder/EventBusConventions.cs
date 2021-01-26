using EasyNetQ;
using System;

namespace DotnetCore.Rpc.Responder
{
    /// <summary>
    /// Instead default naming convention, use QueueAttribute queue name
    /// </summary>
    public sealed class EventBusConventions : Conventions
    {
        public EventBusConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
        {
            ExchangeNamingConvention = type =>
            {
                QueueAttribute MyAttribute = (QueueAttribute)Attribute.GetCustomAttribute(type, typeof(QueueAttribute));
                return MyAttribute.ExchangeName;
            };
            RpcRoutingKeyNamingConvention = type =>
            {
                QueueAttribute MyAttribute = (QueueAttribute)Attribute.GetCustomAttribute(type, typeof(QueueAttribute));
                return MyAttribute.QueueName;
            };
            //ErrorQueueNamingConvention = info => "ErrorQueue";
            //ErrorExchangeNamingConvention = info => "BusErrorExchange_" + info.RoutingKey + assemblyName;
        }
    }
}
