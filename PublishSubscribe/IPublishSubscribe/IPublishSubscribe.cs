/// <summary>
/// Provides an abstract definition of the Observer pattern.  In this definition, a Subscriber is notified of changes to a Topic that is managed by a Publisher.
/// The associations between Subscribers, Topics, and Publishers is maintained by a Broker.  A Publisher registers with the Broker to announce the availability of Topics.
/// Subscribers use the Broker to subscribe to Topics of interest.  When the Publisher changes the Topic, it uses the Broker to publish Topic change notifications
/// to all current Subscribers.
/// The Broker decouples Subscribers from having to locate specific Publishers in order to subscribe to a Topic. The Publisher assigns each Topic it manages a unique
/// Topic Identifer, which can be an arbitrary Comparable type.  The Topic Identifier is subsequently used by Subscriber's to create a subscription. This approach
/// thus supports a completely location-transparent implementation, where Publishers and Subscribers can reside in the same process, in different processes, or on different
/// computers. Such implementations will become increasingly complex, ultimately involving the need to assign communication endpoints to Subscribers and Publishers, as well as
/// marshaling Topics for delivery into separate address spaces.
/// The application is responsible for creating Brokers and providing a mechanism for locating them.  This mechanism could be as simple as a global singleton Broker reference,
/// or it could be a more complex scheme based on searching a distributed repository of Brokers identified by a federated naming convention.
/// </summary>
namespace PublishSubscribe
{
}
