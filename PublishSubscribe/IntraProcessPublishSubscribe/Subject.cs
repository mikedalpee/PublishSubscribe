using System;

namespace PublishSubscribe.IntraProcessPublishSubscribe
{
    public abstract class Subject<T> : ISubject<T> where T : IComparable<T>
    {
        public Subject(T identifier)
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
