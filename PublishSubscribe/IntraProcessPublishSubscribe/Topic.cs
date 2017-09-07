using System;

namespace PublishSubscribe.IntraProcessPublishSubscribe
{
    public abstract class Topic<T> : ITopic<T> where T : IComparable<T>
    {
        public Topic(T identifier)
        {
            Identifier = identifier;
        }
        public T Identifier
        {
            get;
            private set;
        }
    }
}
