using System;
using System.Collections.Generic;

namespace Heibroch.Common
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> action);

        void Unsubscribe<T>(Action<T> action);

        void Publish<T>(T value);
    }

    public class EventBus : IEventBus
    {
        public Dictionary<Type, List<object>> subscribers;
        
        public EventBus()
        {
            subscribers = new Dictionary<Type, List<object>>();
        }

        public void Publish<T>(T value)
        {
            var type = typeof(T);

            if (!subscribers.ContainsKey(type)) return;

            foreach(var actionOjbect in subscribers[type])
            {
                var action = (Action<T>)actionOjbect;
                action(value);
            }
        }

        public void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);

            if (!subscribers.ContainsKey(type))
                subscribers.Add(type, new List<object>());

            var actionList = subscribers[type];
            if (actionList.Contains(action)) return;

            actionList.Add(action);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            var type = typeof(T);

            if (!subscribers.ContainsKey(type)) return;

            var actionList = subscribers[type];
            if (!actionList.Contains(action)) return;

            actionList.Remove(action);
        }

        private void Try<T>(Action<T> action, T value)
        {
            try
            {
                action(value);
            }
            catch
            {
                //Suppress
            }
        }
    }
}
