using System;
using System.Collections.Generic;

namespace Heibroch.Common
{
    public interface IContainer
    {
        void Register<T>(T obj);
        T Resolve<T>();
    }

    public class Container : IContainer
    {
        public static IContainer Current = new Container();

        readonly Dictionary<Type, object> objects = new Dictionary<Type, object>();

        public void Register<T>(T obj)
        {
            objects[typeof(T)] = obj;
        }

        public T Resolve<T>()
        {
            if (!objects.ContainsKey(typeof(T)))
                objects[typeof(T)] = typeof(T).GetConstructors()[0].Invoke(new object[0]);

            return (T)objects[typeof(T)];
        }
    }
}
