using System;
using System.Collections.Generic;

namespace PublishSubscribe.IPublishSubscribe
{
    public abstract class ISingleton<S>
    {
        public class DuplicateNameError : Exception
        {
            public DuplicateNameError(string name)
            :
                base("Singleton " + name + " already exists.")
            {
            }
        }
        public class UndefinedNameError : Exception
        {
            public UndefinedNameError(string name)
            :
                base("Singleton " + name + " does not exist.")
            {
            }
        }

        private static IDictionary<string, S> s_instances = new Dictionary<string, S>();

        public ISingleton(string name,Func<string,S> create)
        {
            if (s_instances.ContainsKey(name))
            {
                throw new DuplicateNameError(name);
            }

            Instances[name] = create(name);
        }
        public S Instance(string name)
        {
            if (! Instances.ContainsKey(name))
            {
                throw new UndefinedNameError(name);
            }

            return Instances[name];
        }
        protected static IDictionary<string, S> Instances
        {
            get
            {
                return s_instances;
            }
        }
   }
}
