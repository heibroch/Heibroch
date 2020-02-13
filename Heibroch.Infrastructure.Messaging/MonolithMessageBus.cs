using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Heibroch.Infrastructure.Messaging
{
    internal class MonolithMessageBus : INetworkMessageBus
    {
        static ConcurrentDictionary<Type, List<Action<object>>> subscribers = new ConcurrentDictionary<Type, List<Action<object>>>();

        public MonolithMessageBus()
        {

        }

        public void Send<T>(T message)
        {
            if (!subscribers.TryGetValue(typeof(T), out var actions)) return;

            lock (actions)
            {
                foreach (var action in actions)
                {
                    try
                    {
                        action(message);
                    }
                    catch
                    {
                        //Log error
                    }
                }
            }
        }

        public void Subscribe<T>(Action<object> action)
        {
            if (!subscribers.TryGetValue(typeof(T), out var subscriberEntries))
            {
                subscriberEntries = new List<Action<object>>();
                subscribers.TryAdd(typeof(T), subscriberEntries);
            }

            lock (subscriberEntries)
            {
                if (subscriberEntries.Contains(action)) return;
                subscriberEntries.Add(action);
            }
        }

        public void Unsubscribe<T>(Action<object> action)
        {
            if (!subscribers.TryGetValue(typeof(T), out var subscriberEntries))
            {
                return;
            }

            lock (subscriberEntries)
            {
                if (!subscriberEntries.Contains(action)) return;
                subscriberEntries.Remove(action);
            }
        }
    }
}
