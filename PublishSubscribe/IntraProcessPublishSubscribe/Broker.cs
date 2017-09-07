using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.IntraProcessPublishSubscribe
{
    public class Broker<T> : IBroker<T> where T : IComparable<T>
    {
        public class PublisherNotRegisteredForTopicException : Exception
        {
        }
        private class Subscription
        {
            public ISubscriber<T> m_subscriber;
            public IPublisher<T>  m_publisher;

            public Subscription(ISubscriber<T> subscriber, IPublisher<T> publisher)
            {
                m_subscriber = subscriber;
                m_publisher = publisher;
            }
        }
        IDictionary<T, IList<Subscription>> m_subscribers = new Dictionary<T, IList<Subscription>>();
        IDictionary<T, IList<IPublisher<T>>>   m_publishers  = new Dictionary<T, IList<IPublisher<T>>>();
        public Broker(string name)
        {
            Name = name;
        }
        public string Name
        {
            get;
            private set;
        }
        public void Register(IPublisher<T> publisher, T identifier)
         {
            // Enable an publisher to publish a particular topic

            IList<IPublisher<T>> publishers;

            if (!m_publishers.ContainsKey(identifier))
            {
                publishers = new List<IPublisher<T>>();

                m_publishers[identifier] = publishers;
            }
            else
            {
                publishers = m_publishers[identifier];
            }

            // Register the publisher if it is not already registered

            if (!publishers.Contains(publisher))
            {
                publishers.Add(publisher);
            }
        }
        public bool IsRegistered(IPublisher<T> publisher, T identifier)
        {
            bool isRegistered = false;

            if (m_publishers.ContainsKey(identifier))
            {
                isRegistered = m_publishers[identifier].Contains(publisher);
            }

            return isRegistered;
        }
        public ulong PublisherCount(T identifier)
        {
            ulong publisherCount = 0;

            if (m_publishers.ContainsKey(identifier))
            {
                publisherCount = (ulong)m_publishers[identifier].Count;
            }

            return publisherCount;
        }
        public ulong PublisherCount()
        {
            IList<IPublisher<T>> uniquePublishers = new List<IPublisher<T>>();

            ulong publisherCount = 0;

            foreach (var pair in m_publishers)
            {
                foreach (var publisher in pair.Value)
                {
                    if (!uniquePublishers.Contains(publisher))
                    {
                        publisherCount++;
                        uniquePublishers.Add(publisher);
                    }
                }
            }

            return publisherCount;
        }
        public IList<IPublisher<T>> Publishers(T identifier)
        {
            IList<IPublisher<T>> publishers = new List<IPublisher<T>>();

            if (m_publishers.ContainsKey(identifier))
            {
                foreach (var publisher in m_publishers[identifier])
                {
                    publishers.Add(publisher);
                }
            }

            return publishers;
        }
        public IList<IPublisher<T>> Publishers()
        {
            IList<IPublisher<T>> uniquePublishers = new List<IPublisher<T>>();

            foreach (var pair in m_publishers)
            {
                foreach (var publisher in pair.Value)
                {
                    if (!uniquePublishers.Contains(publisher))
                    {
                        uniquePublishers.Add(publisher);
                    }
                }
            }

            return uniquePublishers;
        }
        private  void RemovePublisher(IPublisher<T> publisher, T identifier)
        {
            // Remove this publisher from being able to publish this topic

            IList<IPublisher<T>> publishers = m_publishers[identifier];

            if (publishers.Contains(publisher))
            {
                publishers.Remove(publisher);

                if (publishers.Count() == 0)
                {
                    m_publishers.Remove(identifier);
                }
            }

        }
        public void Unregister(IPublisher<T> publisher, T identifier)
        {
            if (m_publishers.ContainsKey(identifier))
            {
                RemovePublisher(publisher, identifier);
            }
        }
        public void Unregister(IPublisher<T> publisher)
        {
            var identifiers = m_publishers.Keys.ToList();

            foreach (var identifier in identifiers)
            {
                RemovePublisher(publisher, identifier);
            }
        }
        public void Unregister()
        {
            var values = m_publishers.Values.ToList();

            foreach (var publishers in values)
            {
                var publishersCopy = publishers.ToList();

                foreach (var publisher in publishersCopy)
                {
                    Unregister(publisher);
                }
            }
        }
        public void Subscribe(ISubscriber<T> subscriber, T identifier)
        {
            // Subscribe to a particular topic

            IList<Subscription> subscriptions;

            if (!m_subscribers.ContainsKey(identifier))
            {
                subscriptions = new List<Subscription>();

                m_subscribers[identifier] = subscriptions;
            }
            else
            {
                subscriptions = m_subscribers[identifier];
            }

            // Subscribe the subscriber if it is not already subscribed

            IEnumerable<Subscription> currentSubscriptions = subscriptions.Where(s => s.m_subscriber == subscriber);

            if (currentSubscriptions.Count() == 0)
            {
                subscriptions.Add(new Subscription(subscriber,null));
            }
            else if (currentSubscriptions.First().m_publisher != null)
            {
                subscriptions = subscriptions.Where(s => s.m_subscriber != subscriber).ToList();

                subscriptions.Add(new Subscription(subscriber, null));
            }
        }
        public void Subscribe(ISubscriber<T> subscriber, T identifier, IPublisher<T> publisher)
        {
            // Subscribe to a particular topic

            IList<Subscription> subscriptions;

            if (!m_subscribers.ContainsKey(identifier))
            {
                subscriptions = new List<Subscription>();

                m_subscribers[identifier] = subscriptions;
            }
            else
            {
                subscriptions = m_subscribers[identifier];
            }

            // Subscribe the subscriber if it is not already subscribed

            IEnumerable<Subscription> currentSubscriptions = subscriptions.Where(s => s.m_subscriber == subscriber);

            if (currentSubscriptions.Count() == 0)
            {
                subscriptions.Add(new Subscription(subscriber, publisher));
            }
            else if (currentSubscriptions.First().m_publisher != null)
            {
                if (currentSubscriptions.Where(s => s.m_publisher == publisher).Count() == 0)
                {
                    subscriptions.Add(new Subscription(subscriber, publisher));
                }
            }
        }
        public bool IsSubscribed(ISubscriber<T> subscriber, T identifier)
        {
            return m_subscribers.ContainsKey(identifier) && m_subscribers[identifier].FirstOrDefault(s => s.m_subscriber == subscriber) != null;
        } 
        public bool IsSubscribed(ISubscriber<T> subscriber)
        {
            foreach (var identifier in m_subscribers.Keys)
            {
                if (m_subscribers[identifier].FirstOrDefault(s => s.m_subscriber == subscriber) != null)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsSubscribed(T identifier)
        {
            return m_subscribers.ContainsKey(identifier);
        }
        public bool IsSubscribed()
        {
            return m_subscribers.Count > 0;
        }
        public IList<ISubscriber<T>> Subscribers(T identifier)
        {
            IList<ISubscriber<T>> subscribers =
                (m_subscribers.ContainsKey(identifier))
                ?
                    m_subscribers[identifier].Select(s => s.m_subscriber).Distinct().ToList()
                :
                    new List<ISubscriber<T>>();

            return subscribers;
        }
        public IList<ISubscriber<T>> Subscribers()
        {
            IList<ISubscriber<T>> uniqueSubscribers = new List<ISubscriber<T>>();
            
            foreach (var key in m_subscribers.Keys)
            {
                IList<ISubscriber<T>> subscribers = Subscribers(key);

                uniqueSubscribers = uniqueSubscribers.Concat(subscribers).Distinct().ToList();
            }

            return uniqueSubscribers;
        }
        public ulong SubscriberCount(T identifier)
        {
            return (ulong)Subscribers(identifier).Count;
        }
        public ulong SubscriberCount()
        {
            return (ulong)Subscribers().Count;
        }
        public void Unsubscribe(ISubscriber<T> subscriber, T identifier)
        {
            if (m_subscribers.ContainsKey(identifier))
            {
                IList<Subscription> subscriptions = m_subscribers[identifier];

                subscriptions = subscriptions.Where(s => s.m_subscriber != subscriber).ToList();

                if (subscriptions.Count == 0)
                {
                    m_subscribers.Remove(identifier);
                }
            }
        }
        public void Unsubscribe(ISubscriber<T> subscriber,T identifier,IPublisher<T> publisher)
        {
            if (m_subscribers.ContainsKey(identifier))
            {
                IList<Subscription> subscriptions = m_subscribers[identifier];

                subscriptions = subscriptions.Where(s => s.m_subscriber != subscriber || s.m_publisher != publisher).ToList();

                if (subscriptions.Count == 0)
                {
                    m_subscribers.Remove(identifier);
                }
            }
        }
        public void Unsubscribe(ISubscriber<T> subscriber)
        {
            foreach (var identifier in m_publishers.Keys)
            {
                Unsubscribe(subscriber, identifier);
            }
        }
        public void Unsubscribe(ISubscriber<T> subscriber, IPublisher<T> publisher)
        {
            foreach (var identifier in m_publishers.Keys)
            {
                Unsubscribe(subscriber, identifier, publisher);
            }
        }
        public void Unsubscribe(T identifier)
        {
            if (m_subscribers.ContainsKey(identifier))
            {
                m_subscribers[identifier].Clear();

                m_subscribers.Remove(identifier);
            }
        }
        public void Unsubscribe(T identifier, IPublisher<T> publisher)
        {
            if (m_subscribers.ContainsKey(identifier))
            {
                IList<Subscription> subscriptions = m_subscribers[identifier];

                subscriptions = subscriptions.Where(s => s.m_publisher != publisher).ToList();

                if (subscriptions.Count == 0)
                {
                    m_subscribers.Remove(identifier);
                }
            }
        }
        public void Unsubscribe(IPublisher<T> publisher)
        {
            foreach (var identifier in m_publishers.Keys)
            {
                Unsubscribe(identifier, publisher);
            }
        }
        public void Unsubscribe()
        {
            foreach (var identifier in m_publishers.Keys)
            {
                Unsubscribe(identifier);
            }
        }
        public void Publish(IPublisher<T> publisher, ITopic<T> topic)
        {
            // A Publisher must explicitly register to enable publishing

            if (!m_publishers.ContainsKey(topic.Identifier))
            {
                throw new PublisherNotRegisteredForTopicException();
            }

            IList<IPublisher<T>> publishers = m_publishers[topic.Identifier];

            if (!publishers.Contains(publisher))
            {
                throw new PublisherNotRegisteredForTopicException();

            }

            if (m_subscribers.ContainsKey(topic.Identifier))
            {
                IEnumerable<Subscription> subscriptions = m_subscribers[topic.Identifier].Where(s => (s.m_publisher == null || s.m_publisher == publisher));

                foreach (var subscription in subscriptions)
                {
                    subscription.m_subscriber.Notify(topic, publisher);
                }
            }
        }
     }
}
