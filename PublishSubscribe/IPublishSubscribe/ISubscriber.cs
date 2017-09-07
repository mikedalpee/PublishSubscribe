using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Subscriber that will receive Subject change notifications from Publishers to which the Subscriber is subscribed.
    /// </summary>
    /// <typeparam name="T">The Subject Identifier type</typeparam>
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
        /// Called when a Publisher publishes change notifications for a Subject Identifier to which this Subscriber is subscribed.
        /// </summary>
        /// <param name="subject">The changed Subject.</param>
        /// <param name="notifier">The Publisher that made the change.</param>
        void Notify(
            ISubject<T> subject,
            IPublisher<T> notifier);
    }
}
