using System;

namespace Heibroch.Infrastructure.Interfaces.MessageBus
{
    public interface IInternalMessageBus
    {
        void Subscribe<T>(Action<T> action) where T : IInternalEvent;

        void Unsubscribe<T>(Action<T> action) where T : IInternalEvent;

        void Publish<T>(T value) where T : IInternalEvent;
    }
}
