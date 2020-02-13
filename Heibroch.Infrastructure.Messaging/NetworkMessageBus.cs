using System;

namespace Heibroch.Infrastructure.Messaging
{
    internal interface INetworkMessageBus
    {
        void Send<T>(T message);

        void Subscribe<T>(Action<object> action);

        void Unsubscribe<T>(Action<object> action);
    }

    internal class NetworkMessageBus : INetworkMessageBus
    {
        public void Send<T>(T message) => throw new NotImplementedException();

        public void Subscribe<T>(Action<object> action) => throw new NotImplementedException();

        public void Unsubscribe<T>(Action<object> action) => throw new NotImplementedException();
    }
}
