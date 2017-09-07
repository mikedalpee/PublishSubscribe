using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe
{
    /// <summary>
    /// Defines a Broker through which Subscribers subscribe to Publishers to receive Subject change notifications.
    /// </summary>
    /// <typeparam name="T">The Subject Identifier type</typeparam>
    public interface IBroker<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the Broker's Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        string Name
        {
            get;
        }
        /// <summary>
        /// Registers a Publisher as being available to publish changes to a Subject Identifier.
        /// </summary>
        /// <param name="publisher">The Publisher.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        void Register(
            IPublisher<T> publisher,
            T identifier);
        /// <summary>
        /// Determines whether the Publisher is registered for the Subject Identifier.
        /// </summary>
        /// <param name="publisher">The Publisher.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <returns>
        ///   <c>true</c> if the Publisher is registered for the Subject Identifier; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(
            IPublisher<T> publisher,
            T identifier);
        /// <summary>
        /// Requests the number of Publishers registered for the Subject Identifier.
        /// </summary>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <returns>The number of Publishers registered for the Subject Identifier</returns>
        ulong PublisherCount(
            T identifier);
        /// <summary>
        /// Requests the number of unique Publishers registered for all Subject Identifiers.
        /// </summary>
        /// <returns>The number of unique Publishers registered for all Subject Identifiers.</returns>
        ulong PublisherCount();
        /// <summary>
        /// Requests the list of Publishers registered for the Subject Identifier.
        /// </summary>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <returns>The list of Publishers registered for the Subject Identifier.</returns>
        IList<IPublisher<T>> Publishers(
            T identifier);
        /// <summary>
        /// Requests the list of unique Publishers registered for all Subject Identifiers.
        /// </summary>
        /// <returns>The list of unique Publishers registered for all Subject Identifiers.</returns>
        IList<IPublisher<T>> Publishers();
        void Unregister(
            IPublisher<T> publisher,
            T identifier);
        /// <summary>
        /// Unregisters a Publisher from being available to publish changes to all current Subject Identifiers.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        void Unregister(
            IPublisher<T> publisher);
        /// <summary>
        /// Unregisters all Publishers from being available to publish changes to all current Subject Identifiers.
        /// </summary>
        void Unregister();
        /// <summary>
        /// Subscribes the Subscriber to receive change notifications from any Publishers registered for the Subject Identifier.
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        void Subscribe(
            ISubscriber<T> subscriber,
            T identifier);
        /// <summary>
        /// Subscribes the Subscriber to receive change notifications from the specific Publisher if it is registered for the Subject Identifier.
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <param name="publisher">The Publisher.</param>
        void Subscribe(
            ISubscriber<T> subscriber,
            T identifier,
            IPublisher<T> publisher);
        /// <summary>
        /// Is the Subscriber subscribed to the Subject Identifier of any Publisher?
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <returns>
        ///   <c>true</c> if the Subscriber is subscribed to the Subject Identifier any Publisher; otherwise, <c>false</c>.
        /// </returns>
        bool IsSubscribed(
            ISubscriber<T> subscriber,
            T identifier);
        /// <summary>
        /// Is the Subscriber subscribed to any Subject Identifier of any Publisher?
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <returns>
        ///   <c>true</c> if the Subscriber is subscribed to any Subject Identifier of any Publisher; otherwise, <c>false</c>.
        /// </returns>
        bool IsSubscribed(
           ISubscriber<T> subscriber);
        /// <summary>
        /// Is any Subscriber subscribed to the Subject Identifer of any Publisher?
        /// </summary>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <returns>
        ///   <c>true</c> any Subscriber is subscribed to the Subject Identifer of any Publisher; otherwise, <c>false</c>.
        /// </returns>
        bool IsSubscribed(
             T identifier);
        /// <summary>
        /// Is any Subscriber subscribed to any Subject Identifier of any Publisher?
        /// </summary>
        /// <returns>
        ///   <c>true</c> if any Subscriber is subscribed to any Subject Identifier of any Publisher; otherwise, <c>false</c>.
        /// </returns>
        bool IsSubscribed();
        /// <summary>
        /// Requests a list of Subcribers subscribed to the Subject Identifier across all Publishers.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list of Subcribers subscribed to the Subject Identifier across all Publishers.</returns>
        IList<ISubscriber<T>> Subscribers(T identifier);
        /// <summary>
        /// Requests a list of Subscribers subscribed to every Subject Identifier across all Publisher.
        /// </summary>
        /// <returns>A list of Subscribers subscribed to every Subject Identifier across all Publisher.</returns>
        IList<ISubscriber<T>> Subscribers();
        /// <summary>
        /// Requests the number of subscriptions to the Subject Identifier across all Publishers.
        /// </summary>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <returns>The number of subscriptions to the Subject Identifier across all Publishers.</returns>
        ulong SubscriberCount(T identifier);
        /// <summary>
        /// Requests the number of subscriptions to all Subject Identifiers across all Publisher.
        /// </summary>
        /// <returns>The number of subscriptions to all Subject Identifiers across all Publisher.</returns>
        ulong SubscriberCount();
        /// <summary>
        /// Unsubscribes a Subscriber from the Subject Identifier.
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        void Unsubscribe(
            ISubscriber<T> subscriber,
            T identifier);
        /// <summary>
        /// Unsubscribes a Subscriber from the Publisher for the Subject Identifier.
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <param name="publihser">The Publisher.</param>
        void Unsubscribe(
            ISubscriber<T> subscriber,
            T identifier,
            IPublisher<T> publisher);
        /// <summary>
        /// Unsubscribes a Subscriber across all Subject Identifiers.
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        void Unsubscribe(
            ISubscriber<T> subscriber);
        /// <summary>
        /// Unsubscribes a Subscriber from the Publisher across all Subject Identifiers.
        /// </summary>
        /// <param name="subscriber">The Subscriber.</param>
        /// <param name="publisher">The Publisher.</param>
        void Unsubscribe(
            ISubscriber<T> subscriber,
            IPublisher<T> publisher);
        /// <summary>
        /// Unsubscribes all Subscribers from the Publisher for the Subject Identifier.
        /// </summary>
        /// <param name="identifier">The Subject Identifier.</param>
        /// <param name="publisher">The Publisher.</param>
        void Unsubscribe(
            T identifier,
            IPublisher<T> publisher);
        /// <summary>
        /// Unsubscribes all Subscribers from the Subject Identifier.
        /// </summary>
        /// <param name="identifier">The Subject Identifier.</param>
        void Unsubscribe(
            T identifier);
        /// <summary>
        /// Unsubscribes all Subscribers from the Publisher across all Subject Identifiers.
        /// </summary>
        /// <param name="publisher">The Publisher.</param>
        void Unsubscribe(
            IPublisher<T> publisher);
        /// <summary>
        /// Unsubscribes all Subscribers from all Subject Identifiers.
        /// </summary>
        void Unsubscribe();
         /// <summary>
        /// Publishes a change notificaton to all Subcribers currently subscribed to the Subject's Subject Identifier.
        /// </summary>
        /// <param name="publisher">The Publisher.</param>
        /// <param name="subject">The Subject.</param>
        void Publish(
            IPublisher<T> publisher,
            ISubject<T> subject);
    }
}
