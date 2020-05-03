using System;
using System.Collections.Generic;

namespace LibraryManagement.Core
{
    /// <summary>
    ///    Generic singleton dictionary
    /// </summary>
    /// <typeparam name="T">The type of object to store</typeparam>
    public class Singleton<T>
    {

        private static T instance;

        private static IDictionary<Type, object> AllSingletons { get; }

        static Singleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }

        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }

    }
}
