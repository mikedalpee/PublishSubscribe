using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Subscriber that will receive Topic change notifications from Publishers to which the Subscriber is subscribed.
    /// </summary>
    /// <typeparam name="T">The Topic Identifier type</typeparam>
    public interface ISubscriber<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the Subscriber's Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        string Name
        {
            get;
        }
        /// <summary>
        /// Called when a Publisher publishes change notifications for a Topic Identifier to which this Subscriber is subscribed.
        /// </summary>
        /// <param name="topic">The changed Topic.</param>
        /// <param name="notifier">The Publisher that made the change.</param>
        void Notify(
            ITopic<T> topic,
            IPublisher<T> notifier);
    }
}
