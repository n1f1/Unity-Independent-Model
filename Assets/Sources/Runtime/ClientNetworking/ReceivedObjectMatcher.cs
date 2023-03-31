using System;
using System.Collections.Generic;
using System.Reflection;
using Networking;

namespace ClientNetworking
{
    public class ReceivedReplicatedObjectMatcher : IReplicatedObjectReceiver<object>
    {
        private readonly Dictionary<Type, object> _receivers;

        public ReceivedReplicatedObjectMatcher(Dictionary<Type, object> receivers)
        {
            _receivers = receivers ?? throw new ArgumentNullException(nameof(receivers));
        }

        public void Receive(object createdObject)
        {
            Type receivedObjectType = createdObject.GetType();
            object receiver = _receivers[receivedObjectType];
            Type receiverType = receiver.GetType();
            MethodInfo method = receiverType.GetMethod("Receive");
            method.Invoke(receiver, new[] {createdObject});
        }
    }
}