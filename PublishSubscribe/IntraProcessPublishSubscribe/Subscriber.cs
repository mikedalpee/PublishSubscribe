using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.IntraProcessPublishSubscribe
{
    public abstract class Subscriber<T> : ISubscriber<T> where T : IComparable<T>
    {
        public Subscriber(string name)
        {
            Name = name;
        }
        public string Name
        {
            get;
            private set;
        }
        public abstract void Notify(ISubject<T> subject, IPublisher<T> notifier);
    }
}
