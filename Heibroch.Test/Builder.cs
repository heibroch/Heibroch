﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace TestHelpers
{
    public class Builder<T>
    {
        private readonly Dictionary<Type, object> objectcontainer;
        private readonly Dictionary<Type, object> mockContainer; 

        public Builder()
        {
            objectcontainer = new Dictionary<Type, object>();
            mockContainer = new Dictionary<Type, object>();

            foreach (var constructor in (typeof (T)).GetConstructors())
            {
                foreach (var parameter in constructor.GetParameters())
                {
                    if (mockContainer.ContainsKey(parameter.ParameterType)) continue;
                    mockContainer.Add(parameter.ParameterType, DynamicMock(parameter.ParameterType));
                }
            }
        }
        
        public Mock<T> ResolveMock<T>() where T : class
        {
            if (!mockContainer.ContainsKey(typeof (T)))
                throw new InvalidOperationException("This class is not dependent on specified type: " + typeof(T));

            try
            {
                return (Mock<T>)mockContainer[typeof(T)];
            }
            catch (InvalidCastException)
            {
                throw new Exception("The mock you're trying to fetch is no longer a mock. Have you registered an instance with the builder?");
            }
        }

        /// <summary>
        /// If an instance is needed instead of a mock it can be specifically registered to replace the mock.
        /// </summary>
        /// <returns></returns>
        public Builder<T> WithInstance<K>(K value)
        {
            if (!objectcontainer.ContainsKey(typeof (K)))
            {
                objectcontainer.Add(typeof(K), value);
                return this;
            }

            objectcontainer[typeof (K)] = value;
            return this;
        }

        public T Build(int constructorIndex = 1)
        {
            var constructors = typeof (T).GetConstructors();
            var constructor = constructors[constructorIndex];           

            var parameters = constructor.GetParameters();

            var args = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i)
            {
                args[i] = objectcontainer.ContainsKey(parameters[i].ParameterType)
                    ? objectcontainer[parameters[i].ParameterType]
                    : GetObjectFromMock(mockContainer[parameters[i].ParameterType], parameters[i].ParameterType);
            }
            
            return (T)constructor.Invoke(args);
        }

        public static object DynamicMock(Type type)
        {
            if (type.IsArray)
            {
                dynamic listOfItems = Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetElementType()));
                return listOfItems.ToArray();
            }
            if (type == typeof(string))
                return String.Empty;

            var mock = typeof(Mock<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            return mock;
        }

        private static object GetObjectFromMock(object mock, Type type)
        {
            return
                mock.GetType()
                    .GetProperties()
                    .Single(f => f.Name == "Object" && f.PropertyType == type)
                    .GetValue(mock, new object[] { });
        }
    }
}
