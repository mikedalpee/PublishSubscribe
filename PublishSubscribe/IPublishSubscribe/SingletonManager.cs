using System;
using System.Collections.Generic;

namespace PublishSubscribe.IPublishSubscribe
{
    public class SingletonManager<S>
    {
        public class DuplicateNameError : Exception
        {
            public DuplicateNameError(string name)
            :
                base("Singleton name " + name + " already exists.")
            {
            }
        }
        public class DuplicateSingletonError : Exception
        {
            public DuplicateSingletonError(string name)
            :
                base("Singleton " + name + " already exists.")
            {
            }
        }
        public class UndefinedNameError : Exception
        {
            public UndefinedNameError(string name)
            :
                base("Singleton name " + name + " does not exist.")
            {
            }
        }

        private static IDictionary<string, S> s_instances = new Dictionary<string, S>();

        public static S Register(string name,S singleton)
        {
            if (s_instances.ContainsKey(name))
            {
                throw new DuplicateNameError(name);
            }

            if (s_instances.Values.Contains(singleton))
            {
                throw new DuplicateSingletonError(name);
            }

            Instances[name] = singleton;

            return singleton;
        }
        public static S Instance(string name)
        {
            if (! Instances.ContainsKey(name))
            {
                throw new UndefinedNameError(name);
            }

            return Instances[name];
        }
        private static IDictionary<string, S> Instances
        {
            get
            {
                return s_instances;
            }
        }
   }
}
