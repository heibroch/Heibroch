using System;
using System.Collections.Generic;
using System.Text;

namespace Heibroch.Infrastructure.Messaging
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
    }

    public interface IMessageBus
    {
        void Send<T>(T message) where T : IMessage;

        void Subscribe<T>(Action<object> action) where T : IMessage;

        void Unsubscribe<T>(Action<object> action) where T : IMessage;

        void RegisterNetworkService(string serviceName);
    }

    public class MessageBus : IMessageBus
    {
        private readonly string serviceName;
        private MonolithMessageBus monolithMessageBus;
        private NetworkMessageBus networkMessageBus;
        private List<string> networkServices;

        public MessageBus(string serviceName)
        {
            this.serviceName = serviceName;

            monolithMessageBus = new MonolithMessageBus();
            networkMessageBus = new NetworkMessageBus();

            networkServices = new List<string>();
        }

        public void RegisterNetworkService(string serviceName)
        {
            if (networkServices.Contains(serviceName)) return;
            networkServices.Add(serviceName);
        }

        public void Send<T>(T message) where T : IMessage
        {
            Console.WriteLine($"{serviceName} sent {typeof(T).Name}");

            if (IsNetworkBound())
                networkMessageBus.Send(message);
            else
                monolithMessageBus.Send(message);
        }

        public void Subscribe<T>(Action<object> action) where T : IMessage
        {
            if (IsNetworkBound())
                networkMessageBus.Subscribe<T>(action);
            else
                monolithMessageBus.Subscribe<T>(action);
        }

        public void Unsubscribe<T>(Action<object> action) where T : IMessage
        {
            if (IsNetworkBound())
                networkMessageBus.Unsubscribe<T>(action);
            else
                monolithMessageBus.Unsubscribe<T>(action);
        }

        private bool IsNetworkBound() => networkServices.Contains(serviceName);
    }
}
